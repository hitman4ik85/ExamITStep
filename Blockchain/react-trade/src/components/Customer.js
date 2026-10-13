// Компонент ролі «Покупець».
// -----------------------------------------------
// 1) Підключення ВЛАСНОГО гаманця (MetaMask) покупця — це повинен бути інший акаунт, ніж у продавця.
// 2) Завантаження списку товарів із контракту (getProducts) — ДОСТУПНО лише у стані Ready.
// 3) Створення замовлення (createOrder(productName)) — фіксує обраний товар і виставляє state -> OrderCreated.
// 4) Оплата замовлення (payOrder) — надсилаємо суму рівно 2 * price (механізм подвійної застави),
//    після чого state -> OrderPaid.
// 5) Завершення замовлення (orderCompleted) — доступно коли продавець підтвердив оплату (Shipment);
// -----------------------------------------------
// Структура станів:
// - Ready -> (createOrder) -> OrderCreated -> (payOrder) -> OrderPaid -> (orderCompleted) -> Shipment -> Ready
// Поділ коштів: 
// - price -> seller, price -> buyer (повернення застави), state -> Ready.

import React from 'react'
import Web3 from 'web3';
import { useState, useEffect } from 'react';
import { CONTRACT_ADDRESS, CONTRACT_ABI, BYTECODE } from '../config';

export default function Customer() {

  // ---- Локальний стан UI ----
  const [address, setAddress] = useState('');     // адреса контракту (читаємо з localStorage, який заповнив Seller)
  const [account, setAccount] = useState('');     // акаунт покупця (MetaMask)
  const [contract, setContract] = useState(null); // об'єкт контракту web3.eth.Contract (тільки читання/виклики)
  const [products, setProducts] = useState([]);   // масив товарів [{product, price}] для таблиці
  const [appState, setAppState] = useState(0);    // поточний state контракту (enum State як число)
  const [order, setOrder] = useState('');         // назва обраного для купівлі товару
  const [sellerAddr, setSellerAddr] = useState(''); // адреса продавця (щоб заборонити покупку своїм же акаунтом)

  // баланс контракту
  const [balanceWei, setBalanceWei] = useState('0');

  // баланси EOA (гаманець покупця та продавця)
  const [buyerEthWei, setBuyerEthWei] = useState('0');
  const [sellerEthWei, setSellerEthWei] = useState('0');

  // Людське відображення enum State контракту
  const state = ['Ready', 'OrderCreated', 'OrderPaid', 'Shipment', 'OrderCompleted'];

  // 1) Беремо адресу контракту з localStorage
  // Цю адресу надав Seller після підключення/деплою, щоб обидві ролі працювали з одним екземпляром.
  useEffect( () => {
    const ad = JSON.parse(localStorage.getItem('address'));
    if(ad) {
      setAddress(ad);
    } else {
      console.log("No address found in localStorage");
    }
  }, []);

  // 2) Коли відома адреса контракту — створюємо методи контракту і одразу читаємо базову інформацію
  useEffect( () => {
    console.log('buyer contract address');
    console.log(address);
    deployContract();
  }, [address]);

  // зчитуємо баланс контракту (view getBalance)
  const refreshBalance = async (instance, from) => {
    const c = instance || contract;
    if (!c) return;
    try {
      const b = await c.methods.getBalance().call(from ? { from } : {});
      setBalanceWei(String(b));
    } catch (e) {
      console.log('getBalance error', e);
    }
  };

  // зчитувємо баланси гаманців (EOA) покупця та продавця
  const refreshEOABalances = async (acct, seller) => {
    if (!window.ethereum?.isMetaMask) return;
    const w3 = new Web3(window.ethereum);
    try {
      if (acct) {
        const b = await w3.eth.getBalance(acct);
        setBuyerEthWei(String(b));
      }
      if (seller) {
        const s = await w3.eth.getBalance(seller);
        setSellerEthWei(String(s));
      }
    } catch(e){
      console.log('EOA balance error', e);
    }
  };

  // Створення методів контракту на відомій адресі + читання state і seller
  const deployContract = async () => {
    if(window.ethereum?.isMetaMask && address) {
      const w3 = new Web3(window.ethereum);
      const myContract = new w3.eth.Contract(CONTRACT_ABI, address);
      setContract(myContract);
      console.log(myContract);

      // Одразу читаємо поточний стан та адресу продавця (для валідацій)
      try{
        const s = await myContract.methods.state().call();
        setAppState(Number(s));
        const sel = await myContract.methods.seller().call();
        setSellerAddr(sel);
        // після підключення до контракту одразу показуємо баланс
        await refreshBalance(myContract);
        // і підтягнемо баланси гаманців (якщо акаунт підключать пізніше — оновиться ще раз)
        await refreshEOABalances(account || null, sel);
      }catch(e){
        console.log('state/seller read error', e);
      }
    }
  }

  // 3) Покупець підключає СВІЙ гаманець (інший акаунт, ніж у продавця)
  const connectWallet = async () => {
    if(window.ethereum?.isMetaMask) {
      const w3 = new Web3(window.ethereum);
      const acc = await w3.eth.requestAccounts();    // MetaMask звернеться за дозволом
      if (acc && acc[0]) setAccount(acc[0]);

      // Після підключення акаунта перечитуємо state/seller
      if (contract){
        try{
          const s = await contract.methods.state().call({from:acc[0]});
          setAppState(Number(s));
          const sel = await contract.methods.seller().call({from:acc[0]});
          setSellerAddr(sel);
          // оновлюємо баланс після конекту гаманця
          await refreshBalance(contract, acc[0]);
          // та баланси гаманців
          await refreshEOABalances(acc[0], sel);
        }catch(e){ console.log(e); }
      }
    }
  }

  // 4) Завантаження списку товарів (доступно лише у Ready)
  // Контракт повертає JSON-рядок, парсимо його в масив для відображення у таблиці.
  const getProducts = async () => {
    if( contract && account ){
      const response = await contract.methods.getProducts().call({from:account});
      console.log(response);
      const parsed = JSON.parse(response);
      setProducts(parsed);
      await getState(); // оновлюємо стан після запиту
      // баланс тут не змінюється
    }
  }

  // 5) Щоб Прочитати поточний state(стан) контракту (0..4)
  const getState = async () => {
    if(contract){
      const response = await contract.methods.state().call({from:account});
      console.log("getState", response);
      setAppState(Number(response));
    }
  }

  // 6) Для створення замовлення на товар
  // Заборона купувати своєю ж адресою продавця.
  const createOrder = async (productName) => {
    if( contract && account ){
      // Блокування покупки своїм же акаунтом
      if (sellerAddr && account.toLowerCase() === sellerAddr.toLowerCase()){
        alert('Цим акаунтом ви є продавець. Переключіться на інший аккаунт у MetaMask і натисніть Connect Wallet.');
        return;
      }
      // Виклик смарт-контракту: createOrder(productName) -> state: Ready -> OrderCreated
      await contract.methods.createOrder(productName).send({from:account});
      setOrder(productName);
      await getState();
    }
  }

  // 7) Оплатити замовлення
  // Відправляємо рівно 2 * price як value (механізм подвійної застави: price продавцю + price назад покупцю при завершенні).
  const payOrder = async () => {
    if(contract && account){
      const selected = products.find(p => p.product === order);
      if (!selected){
        alert('Спочатку оберіть товар (Buy), щоб зберегти назву замовлення.');
        return;
      }
      const price = selected.price;
      // Контракт перевіряє: msg.value == 2 * price
      await contract.methods.payOrder().send({from:account, value: 2 * price});
      await getState(); // Перехід у OrderPaid
      await refreshBalance(); // баланс збільшився після оплати
      // у покупця стало менше, у продавця — без змін (кошти на контракті)
      await refreshEOABalances(account, sellerAddr);
    }
  }

  // 8) Завершити замовлення
  // Доступно у Shipment (після confirmPayment від продавця).
  // Контракт розподіляє кошти і повертається у Ready.
  const orderCompleted = async () => {
    if(contract && account){
      await contract.methods.orderCompleted().send({from:account});
      await getState();
      await refreshBalance(); // баланс зменшився після розподілу
      // продавцю додалось price, покупцю повернулась застава price
      await refreshEOABalances(account, sellerAddr);
    }
  }

  // Допоміжні функції для Сustomer UI
  const canLoadProducts = contract && appState === 0; // getProducts тільки у Ready
  const sameAsSeller = sellerAddr && account && (account.toLowerCase() === sellerAddr.toLowerCase()); // покупець не може купувати своїм же акаунтом

  return (
    <div className="min-vh-100 bg-gradient-calm text-on-dark">
      {/* Інформаціядля покупця: поточний state контракту, хто продавець, і хто ми */}
      <div className='row justify-content-center d-flex'>
        <div className='left-div col-md-9 col-lg-9 col-sm-8'>
          { contract &&
            <div className='alert alert-info'>Contract State: {state[appState]}</div>
          }
          { sellerAddr &&
            <div className='alert alert-secondary'>Seller: {sellerAddr}</div>
          }
          { account &&
            <div className='alert alert-success'>Customer: {account}</div>
          }
          {/* показуємо баланс контракту */}
          { contract && (
            <div className='alert alert-light border'>
              {/* переводимо wei в ETH */}
              <div><b>Contract Balance:</b> {balanceWei} wei (~ {Web3.utils.fromWei(balanceWei || '0', 'ether')} ETH)</div>
              {/* показуємо баланси EOA */}
              <div><b>Your Balance:</b> {buyerEthWei} wei (~ {Web3.utils.fromWei(buyerEthWei || '0', 'ether')} ETH)</div>
              { sellerAddr && (
                <div><b>Seller Balance:</b> {sellerEthWei} wei (~ {Web3.utils.fromWei(sellerEthWei || '0', 'ether')} ETH)</div>
              )}
            </div>
          )}
          { sameAsSeller &&
            <div className='alert alert-warning'>
              Ви залогінились акаунтом продавця. Для покупки підключіть інший акаунт MetaMask.
            </div>
          }
        </div>
      </div>

      {/* Кнопки: підключити гаманець та завантажити товари (тільки у Ready) */}
      <div className='row justify-content-center d-flex'>
        { !account &&
          <button className='btn btn-outline-warning m-1 w-auto' onClick={connectWallet}>
            {/* Підключення гаманця акаунт ПОКУПЦЯ у MetaMask */}
            Connect Wallet
          </button>
        }
        { canLoadProducts &&
          <button className='btn btn-warning m-1 w-auto' onClick={getProducts}>
            {/* Читаємо список товарів getProducts() (доступно у Ready) */}
            Load Products
          </button>
        }
      </div>

      {/* Таблиця товарів з кнопками Buy: обираємо товар, чим зберігаємо його назву у state.order */}
      {products.length > 0 && (
        <div className='row justify-content-center mt-4'>
          <table className='table table-striped w-auto'>
            <thead>
              <tr>
                <th>Product</th>
                <th>Price</th>
                <th>Buy</th>
              </tr>
            </thead>
            <tbody>
              {products.map((item, index) => (
                  <tr key={index}>
                      <td>{item.product}</td>
                      <td>{item.price}</td>
                      <td>
                        <button
                          className='btn btn-outline-primary btn-sm'
                          onClick={() => createOrder(item.product)}
                          disabled={sameAsSeller} // блокуємо покупку своїм же акаунтом (UI-захист)
                        >
                          Buy
                        </button>
                      </td>
                  </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Кнопка оплати: доступна тільки у OrderCreated */}
      { contract && appState === 1 && (
          <button className='btn btn-success m-1' onClick={payOrder}>
            {/* payOrder() з value = 2 * price -> state: OrderPaid */}
            Pay Order
          </button>
      )}

      {/* Кнопка завершення: доступна тільки у Shipment (після Confirm Payment від продавця) */}
      { contract && appState === 3 && (
          <button className='btn btn-primary m-1' onClick={orderCompleted}>
            {/* orderCompleted() -> виплати + state: Ready */}
            Complete Order
          </button>
      )}
    </div>
  )
}