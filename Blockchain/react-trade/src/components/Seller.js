// Компонент ролі «Продавець»:
// -----------------------------------------------
// 1) Підключення гаманця (MetaMask) продавця.
// 2) Підключення/деплой контракту (ми спочатку намагаємося під’єднатися до існуючого,
//    а якщо адреса невалідна — пропонуємо деплой).
// 3) Створення товарів: createProducts(product, price) -> товари зберігаються в mapping у контракті.
// 4) Перегляд товарів: getProducts() (доступно лише у стані Ready, повертає JSON-рядок).
// 5) Підтвердження оплати продавцем: confirmPayment() (кнопка з’являється у стані OrderPaid).
//
// Примітки:
// - Адреса контракту зберігається в localStorage, щоб і Seller, і Customer працювали з ОДНИМ екземпляром.
// - Перед підключенням перевіряємо, що за адресою є байткод (hasCode) який зберігається у блокчейні, і що це саме наш контракт (isOurContract).
//   Це запобігає помилці при неправильній адресі/ABI.
//
// Структура станів:
// Ready -> (createOrder) -> OrderCreated -> (payOrder) -> OrderPaid -> (confirmPayment) -> Shipment -> (orderCompleted) -> Ready
// -----------------------------------------------

import React from 'react';
import Web3 from 'web3';
import { useState } from 'react';
import { CONTRACT_ADDRESS, CONTRACT_ABI, BYTECODE } from '../config';

export default function Seller() {

  // Локальний стан компонента Seller:
  const [accounts, setAccounts] = useState([]);   // акаунти з MetaMask, використовуємо accounts[0] як активний
  const [contract, setContract] = useState(null); // об'єкт контракту web3.eth.Contract (після attach/deploy)
  const [address, setAddress] = useState('');     // адреса активного контракту
  const [product, setProduct] = useState('');     // поле вводу назви товару
  const [price, setPrice] = useState();           // поле вводу ціни (uint256, у тій самій одиниці, що й у контракті)
  const [appState, setAppState] = useState(0);    // числове відображення enum State з контракту
  const [products, setProducts] = useState([]);   // масив для рендеру таблиці товарів
  const [balanceWei, setBalanceWei] = useState('0'); // баланс контракту (в wei)

  // баланси гаманців (EOA) + адреса покупця з контракту
  const [sellerEthWei, setSellerEthWei] = useState('0'); // баланс продавця (EOA) в wei
  const [buyerEthWei, setBuyerEthWei] = useState('0');   // баланс покупця (EOA) в wei
  const [buyerAddr, setBuyerAddr] = useState('');        // адреса покупця з контракту

  // Людське відображення станів (індекси відповідають enum State у контракті)
  const state = ['Ready', 'OrderCreated', 'OrderPaid', 'Shipment', 'OrderCompleted'];

  // Зчитування балансу контракту (view-виклик getBalance())
  const refreshBalance = async (instance) => {
    const c = instance || contract;
    if (!c) return;
    try {
      const from = (accounts && accounts[0]) || undefined;
      const b = await c.methods.getBalance().call({ from });
      setBalanceWei(String(b));
    } catch (e) {
      console.log('getBalance error', e);
    }
  };

  // зчитуємо баланси гаманців продавця та покупця
  const refreshEOABalances = async () => {
    if (!window.ethereum?.isMetaMask) return;
    const w3 = new Web3(window.ethereum);

    // баланс продавця = поточний accounts[0]
    if (accounts && accounts[0]) {
      try {
        const s = await w3.eth.getBalance(accounts[0]);
        setSellerEthWei(String(s));
      } catch (e) { console.log('seller getBalance error', e); }
    }

    // покупця читаємо з контракту (public buyer)
    if (contract) {
      try {
        const bAddr = await contract.methods.buyer().call();
        if (bAddr && bAddr !== '0x0000000000000000000000000000000000000000') {
          setBuyerAddr(bAddr);
          const b = await w3.eth.getBalance(bAddr);
          setBuyerEthWei(String(b));
        } else {
          setBuyerAddr('');
          setBuyerEthWei('0');
        }
      } catch (e) { console.log('buyer addr/balance read error', e); }
    }
  };

  // Перевірка наявності байткоду за адресою (захист від підключення до порожньої адреси)
  const hasCode = async (w3, addr) => {
    if (!addr) return false;
    try {
      const code = await w3.eth.getCode(addr);
      return code && code !== '0x' && code !== '0x0'; // '0x' означає, що коду за адресою немає
    } catch {
      return false;
    }
  };

  // Додаткова перевірка, що це саме наш контракт (є метод state() і він викликається)
  // Це для того, якщо за адресою справді є контракт, але з іншим ABI.
  const isOurContract = async (c, from) => {
    try {
      await c.methods.state().call({ from });
      return true;
    } catch {
      return false;
    }
  };

  // Функція для відʼєднання контракту локально + видалення localStorage
  const detachContract = () => {
    setContract(null);
    setAddress('');
    setBalanceWei('0'); // Обнуляємо показник балансу
    // чистимо відомості про покупця
    setBuyerAddr('');
    setBuyerEthWei('0');
    localStorage.removeItem('address');
  };

  // Функція підключення до вже існуючого контракту (localStorage або config), якщо він валідний
  const attachExisting = async () => {
    if (!window.ethereum?.isMetaMask) return;
    const w3 = new Web3(window.ethereum);

    // 1) Беремо адресу з localStorage, якщо її немає — беремо CONTRACT_ADDRESS з config
    const saved = JSON.parse(localStorage.getItem('address')) || CONTRACT_ADDRESS;
    if (!saved) return;

    // 2) Перевіряємо, що за адресою є байткод(захист від підключення до порожньої адреси)
    if (!(await hasCode(w3, saved))) {
      detachContract();
      return;
    }

    const c = new w3.eth.Contract(CONTRACT_ABI, saved);

    // 3) Перевіряємо, чи це наш контракт (викликається state())
    const from = (accounts && accounts[0]) || undefined;
    if (!(await isOurContract(c, from))) {
      detachContract();
      return;
    }

    // 4) Якщо все ок — фіксуємо адресу та об'єкт контракту
    setAddress(saved);
    setContract(c);

    // 5) Одразу робимо запит state(), щоб показати актуальні кнопки/статус
    try { await getState(c); } catch (e) { console.log('getState error', e); }
    // і одразу підтягуємо баланс
    try { await refreshBalance(c); } catch (e) { console.log(e); }
    // і баланси EOA
    try { await refreshEOABalances(); } catch (e) { console.log(e); }

    // 6) Підвантажити товари (доступно тільки у Ready; якщо інший стан — просто пропускаємо)
    try {
      const resp = await c.methods.getProducts().call({ from });
      if (resp) setProducts(JSON.parse(resp));
    } catch {
      console.log('getProducts error (state not Ready?)');
    }
  };

  // Для кнопки "Connect Wallet": підключення акаунта продавця (MetaMask)
  const connectWallet = async () => {
    if (window.ethereum?.isMetaMask) {
      const w3 = new Web3(window.ethereum);
      const acc = await w3.eth.requestAccounts(); // MetaMask відкриє вікно підтвердження
      setAccounts(acc);
      // Після вибору акаунта пробуємо під'єднатися до вже існуючого контракту
      try { await attachExisting(); } catch(e) { console.log(e); }
      // одразу покажемо баланси EOA(продавця та покупця, якщо є)
      try { await refreshEOABalances(); } catch(e) { console.log(e); }
    }
  };

  // Для кнопки "Connect To Contract": приєднатися до валідної адреси АБО задеплоїти новий контракт
  const connectContract = () => {
    if (!window.ethereum?.isMetaMask) return;
    const w3 = new Web3(window.ethereum);

    // 1) Якщо є адреса (localStorage або config) — намагаємось приєднатись, але тільки якщо адреса валідна
    const saved = JSON.parse(localStorage.getItem('address')) || CONTRACT_ADDRESS;
    if (saved) {
      (async () => {
        const codeOk = await hasCode(w3, saved);
        if (codeOk) {
          const c = new w3.eth.Contract(CONTRACT_ABI, saved);
          const ours = await isOurContract(c, accounts[0]);
          if (ours) {
            // якщо адреса валідна та ABI збігається — просто під’єднуємося
            setAddress(saved);
            setContract(c);
            localStorage.setItem('address', JSON.stringify(saved));
            try { await getState(c); } catch(e) { console.log(e); }
            // Показати баланс після підключення
            try { await refreshBalance(c); } catch(e) { console.log(e); }
            // показаті EOA-баланси
            try { await refreshEOABalances(); } catch(e) { console.log(e); }
            return;
          }
        }
        // 2) Якщо адреса не підходить — деплоїмо новий екземпляр контракту
        const myContract = new w3.eth.Contract(CONTRACT_ABI);
        const deployer = myContract.deploy({ data: BYTECODE });
        deployer.send({ from: accounts[0], gas: 20000000 })
          .on('receipt', async (receipt) => {
            const deployedContract = new w3.eth.Contract(CONTRACT_ABI, receipt.contractAddress);
            setAddress(receipt.contractAddress);
            localStorage.setItem('address', JSON.stringify(receipt.contractAddress)); // зберігаємо для Customer
            localStorage.setItem('account', JSON.stringify(accounts[0]));
            setContract(deployedContract);
            try { await getState(deployedContract); } catch(e){ console.log(e); }
            // Баланс одразу після деплою (буде 0)
            try { await refreshBalance(deployedContract); } catch(e){ console.log(e); }
            try { await refreshEOABalances(); } catch(e){ console.log(e); }
          })
          .on('error', (error) => console.error("Error deploying contract:", error));
      })();
      return;
    }

    // 3) Якщо взагалі немає адреси — теж деплоїмо
    const myContract = new w3.eth.Contract(CONTRACT_ABI);
    const deployer = myContract.deploy({ data: BYTECODE });
    deployer.send({ from: accounts[0], gas: 20000000 })
      .on('receipt', async (receipt) => {
        const deployedContract = new w3.eth.Contract(CONTRACT_ABI, receipt.contractAddress);
        setAddress(receipt.contractAddress);
        localStorage.setItem('address', JSON.stringify(receipt.contractAddress));
        localStorage.setItem('account', JSON.stringify(accounts[0]));
        setContract(deployedContract);
        try { await getState(deployedContract); } catch(e){ console.log(e); }
        // Баланс після деплою
        try { await refreshBalance(deployedContract); } catch(e){ console.log(e); }
        try { await refreshEOABalances(); } catch(e){ console.log(e); }
      })
      .on('error', (error) => console.error("Error deploying contract:", error));
  };

  // Додати товар (юзкейс продавця)
  // Викликаємо createProducts(product, price) у контракті.
  // Після додавання очищаємо поля та оновлюємо таблицю.
  const addProduct = async () => {
    if (product && price && contract) {
      await contract.methods.createProducts(product, price).send({ from: accounts[0] });
      setProduct('');
      setPrice('');
      await getProducts();
    }
  };

  // Отримати список товарів із контракту (JSON -> масив), доступно лише у стані Ready (контракт має модифікатор inState(Ready)).
   const getProducts = async () => {
    if (contract) {
      try {
        const response = await contract.methods.getProducts().call({ from: accounts[0] });
        setProducts(JSON.parse(response));
      } catch(e) {
        // Якщо state != Ready — це очікувано кине помилку, ми її ловимо замість crash'у
        console.log('getProducts error (state not Ready?)', e);
      }
      await getState(); // оновлюємо індикатор стану після запиту
    }
  };

  // Прочитати поточний стан контракту (0..4)
  const getState = async (instance) => {
    const c = instance || contract;
    if (c) {
      const response = await c.methods.state().call({ from: accounts[0] });
      setAppState(Number(response));
    }
  };

  // Підтвердити оплату продавцем (доступно у OrderPaid)
  const confirmPayment = async () => {
    if (contract) {
      await contract.methods.confirmPayment().send({ from: accounts[0] });
      await getState();       // після цього стане Shipment
      await refreshBalance(); // для консистентності оновимо баланс (balance тут, як правило, не змінюється)
      // оновимо також EOA-баланси на випадок руху коштів у мережі
      await refreshEOABalances();
    }
  };

  // Для кнопки «Reset Contract»: локально "забути" адресу (для швидкого ресету)
  const onResetContract = () => {
    detachContract();
  };

  return (
    <div className="min-vh-100 bg-gradient-calm text-light">
      {/* Підключення гаманця (зліва) + індикатор стану контракту (справа) */}
      <div className='row justify-content-center d-flex'>
          { !accounts[0] &&
              <button
                className='btn btn-outline-warning col-md-3 col-lg-3 col-sm-4 m-1'
                onClick={connectWallet}
              >
                {/* Підключємо акаунт продавця у MetaMask */}
                Connect Wallet
              </button>
          }

          <div className='left-div col-md-9 col-lg-9 col-sm-8'>
              { accounts[0] &&
                  <div className='alert alert-success'>
                    Connected to {accounts[0]} {/* показуємо, під яким адресом зараз працює продавець */}
                    {/* баланс продавця */}
                    <div className='small text-muted'>
                      {/* переводимо wei в ETH */}
                      <b>Your Balance:</b> {sellerEthWei} wei (~ {Web3.utils.fromWei(sellerEthWei || '0', 'ether')} ETH)
                    </div>
                  </div>
              }
          </div>

          <div className='right-div col-md-3 col-lg-3 col-sm-4'>
              { contract &&
                  <div className='alert alert-info'>
                    {state[appState]} {/* показник: Ready / OrderCreated / ... */}
                  </div>
              }
          </div>
      </div>

      {/* Блок "Contract": тут або показуємо адресу, або кнопку "Connect To Contract" */}
      { accounts[0] &&
        <div className='row justify-content-center d-flex'>
          <div className='col-md-9 col-lg-9 col-sm-8'>
            <div className='alert alert-secondary m-1'>
              { contract ? (
                <div>
                  <div><b>Contract:</b> {address}</div> {/* адреса активного контракту */}
                  {/* показ балансу (wei + приблизно ETH) */}
                  <div className='small text-muted'>
                    <b>Contract Balance:</b> {balanceWei} wei (~ {Web3.utils.fromWei(balanceWei || '0', 'ether')} ETH)
                  </div>
                  {/* покупець (якщо вже зʼявився) */}
                  {buyerAddr && (
                    <div className='small text-muted'>
                      <b>Buyer:</b> {buyerAddr} — {buyerEthWei} wei (~ {Web3.utils.fromWei(buyerEthWei || '0', 'ether')} ETH)
                    </div>
                  )}
                  {/* ручне оновлення балансів */}
                  <button className='btn btn-outline-secondary btn-sm m-1' onClick={refreshEOABalances}>
                    Refresh Balances
                  </button>
                  <button className='btn btn-outline-danger btn-sm m-1' onClick={onResetContract}>
                    {/* Для швидкого перезапуску: забути адресу і заново "Connect To Contract" */}
                    Reset Contract
                  </button>
                </div>
              ) : (
                <div>
                  <div className='mb-2'><b>Contract:</b> not connected</div>
                  <button
                    className='btn btn-outline-warning col-md-3 col-lg-3 col-sm-4 m-1'
                    onClick={connectContract}
                  >
                    {/* Або приєднуємось до валідної адреси, або задеплоїмо новий контракт */}
                    Connect To Contract
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      }

      {/* Форма створення товарів + таблиця списку + Confirm Payment при OrderPaid */}
      <div className='row justify-content-center d-flex mt-4'>
          <div className='col-md-6'>

              {/* 1) Продавець додає товари у публічний список контракту */}
              <input
                type='text'
                className="form-control m-1"
                onChange={(e) => setProduct(e.target.value)}
                value={product}
                placeholder='Enter product name'
              />
              <input
                type='number'
                className="form-control m-1"
                onChange={(e) => setPrice(Number(e.target.value))}
                value={price || ''}
                placeholder='Enter product price'
              />
              <button className='btn btn-outline-info m-1 w-auto' onClick={addProduct}>
                {/* Цей виклик робить createProducts(product, price) */}
                Add Product
              </button>

              <hr/>

              {/* 2) Відображення списку існуючих товарів (парсимо JSON з getProducts()) */}
              {products.length > 0 && (
                  <table className='table table-striped'>
                      <thead>
                          <tr><th>Product</th><th>Price</th></tr>
                      </thead>
                      <tbody>
                          {products.map((item, index) => (
                              <tr key={index}>
                                  <td>{item.product}</td>
                                  <td>{item.price}</td>
                              </tr>
                          ))}
                      </tbody>
                  </table>
              )}

              {/* 3) Підтвердження оплати продавцем — після оплати покупця */}
              { contract && appState === 2 && (
                  <button className='btn btn-info m-1' onClick={confirmPayment}>
                    {/* Викликємо confirmPayment(), state -> Shipment */}
                    Confirm Payment
                  </button>
              )}
          </div>
      </div>
    </div>
  )
}