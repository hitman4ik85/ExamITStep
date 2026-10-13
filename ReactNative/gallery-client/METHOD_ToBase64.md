# METHOD_ToBase64.md

У цьому файлі зібрані різні варіанти перетворення зображення у формат **base64**, які ми пробували у проєкті `gallery-client` (Expo).  
Також описано, **чому вони не спрацювали** в нашому випадку та **що потрібно зробити**, щоб реально їх протестувати.

---

## 1) Варіанти через `react-native-fs` (RNFS)

```ts
/*
    // 1) Варіант через react-native-fs (RNFS)
    // Потрібно: npm install react-native-fs

    // import RNFS from 'react-native-fs';

    const imageToBase64WithRNFS = async () => {
        try {
            if (!file) return;
            const base64Data = await RNFS.readFile(file, 'base64');
            // setBase64(base64Data);
            console.log('RNFS base64 length:', base64Data.length);
        }
        catch (error) {
            console.log('Error converting image to base64 with RNFS:', error);
        }
    };
*/

/*
    // 2) Ще один варіант через RNFS з then/catch

    // import RNFS from 'react-native-fs';

    const imageToBase64WithRNFS_2 = async () => {
        if (!file) return;
        RNFS.readFile(file, 'base64')
            .then(res => {
                console.log('RNFS base64:', res);
                // setBase64(res);
            })
            .catch(err => {
                console.log('RNFS error:', err.message, err.code);
            });
    };
*/
```

### Яку помилку отримаємо

```text
Cannot read properties of undefined (reading 'RNFSFileTypeRegular')
```

### Що це означає

- `RNFS` = `undefined`, тобто бібліотека **`react-native-fs` фізично не підключена як native-модуль** у нашому застосунку.
- Ми зараз працюємо у проєкті, створеному через:

  ```bash
  npx create-expo-app gallery-client
  ```

- Це **Expo (managed workflow)**. В **Expo Go**:
  - Працюють тільки ті нативні модулі, які вже вбудовані в Expo (наприклад, `expo-image-picker`, `expo-device` тощо).
  - Такі пакети, як **`react-native-fs`** і **`rn-fetch-blob`**, вимагають **native-лінкування** (Android/iOS-проєкт), чого в чистому Expo Go немає.

Тому при імпорті:

```ts
import RNFS from 'react-native-fs';
```

всередині виходить, що `RNFS = undefined`, і коли код бібліотеки намагається звернутися до `RNFSFileTypeRegular`, ми отримуємо помилку:

```text
Cannot read properties of undefined (reading 'RNFSFileTypeRegular')
```

Це **обмеження Expo Go**, а не помилка нашого коду.

### Що потрібно зробити, щоб протестувати RNFS

Щоб ці варіанти реально запрацювали, потрібно:
- **Або** зробити:

  ```bash
  expo prebuild
  expo run:android
  # або
  expo run:ios
  ```

  (тобто зібрати **custom dev client**, а не запускати застосунок через стандартний Expo Go),

- **Або** створити **bare React Native**-проєкт (НЕ Expo), наприклад:

  ```bash
  npx react-native init Base64TestApp
  cd Base64TestApp
  npm install react-native-fs
  ```

і вже там тестувати ці методи.

---

## 2) Варіанти через `rn-fetch-blob` (RNFetchBlob)

```ts
/*
    // 3) Варіант через rn-fetch-blob (RNFetchBlob)
    // Потрібно: npm install rn-fetch-blob

    // import RNFetchBlob from 'rn-fetch-blob';

    const imageToBase64WithFetchBlob = () => {
        if (!file) return;
        RNFetchBlob.fetch('GET', file)
            .then(res => {
                let base64String = res.base64();
                console.log('RNFetchBlob base64:', base64String);
                // setBase64(base64String);
            })
            .catch(err => {
                console.log('RNFetchBlob error:', err);
            });
    };
*/

/*
    // 4) Ще один варіант через rn-fetch-blob з readFile("base64")

    // import RNFetchBlob from 'rn-fetch-blob';

    const imageToBase64WithFetchBlob_2 = async () => {
        try {
            if (!file) return;
            return RNFetchBlob.config({ fileCache: true })
                .fetch('GET', file)
                .then((response) => response.readFile('base64'))
                .then((base64Str) => {
                    console.log('RNFetchBlob base64 string:', base64Str);
                    // setBase64(base64Str);
                })
                .catch((error) => {
                    console.error('RNFetchBlob error:', error);
                });
        }
        catch (error) {
            console.error('Error converting image to base64 with RNFetchBlob:', error);
            return null;
        }
    };
*/
```

### Яку помилку отримаємо

```text
Unable to resolve module lodash from ...\node_modules\rn-fetch-blob\index.js: lodash could not be found...
```

### Що це означає

1. Всередині `rn-fetch-blob` є рядок:

   ```ts
   import lodash from 'lodash';
   ```

2. У нашому проєкті **нема пакета `lodash`**, тому Metro-бандлер не може його знайти і крашиться.

Формально це можна виправити командою:

```bash
npm install lodash
```

але це **не вирішує головну проблему**.

---

## 3) Головна проблема: Expo + `rn-fetch-blob` + `react-native-fs`

Ми працюємо в **Expo (managed workflow)**:

```bash
npx create-expo-app gallery-client
```

У такому режимі:

- **Expo Go** включає тільки обмежений набір нативних модулів (ImagePicker, Device, FileSystem, інші `expo-*` пакети).
- **`rn-fetch-blob`** і **`react-native-fs`** – це **нативні модулі**, які потрібно підключати у проєкт Android/iOS:
  - або в **bare React Native**,  
  - або в Expo після **prebuild + custom dev client**.

Тому:

- Спочатку ми впираємось у помилку `react-native-fs`  
  → `RNFS = undefined` → `RNFSFileTypeRegular`.
- Потім у випадку `rn-fetch-blob`:
  - спочатку скарга на відсутній `lodash`,
  - а потім, навіть якщо `lodash` поставити, будуть інші проблеми в Expo Go через те, що native-частина не підключена.

---

## 4) Що потрібно зробити, щоб протестувати ці варіанти

### Варіант 1: Expo prebuild + custom dev client

1. У нашому існуючому Expo-проєкті виконати:

   ```bash
   npx expo prebuild
   ```

2. Встановити бібліотеки:

   ```bash
   npm install react-native-fs rn-fetch-blob lodash
   ```

3. Зібрати свій dev-клієнт:

   ```bash
   npx expo run:android
   # або
   npx expo run:ios
   ```

4. Відкривати застосунок не через Expo Go, а через **custom dev client**.

---

### Варіант 2: окремий bare React Native-проєкт

1. Створити новий проєкт:

   ```bash
   npx react-native init Base64TestApp
   cd Base64TestApp
   ```

2. Встановити:

   ```bash
   npm install react-native-fs rn-fetch-blob lodash
   ```

3. Запустити:

   ```bash
   npx react-native run-android
   # або
   npx react-native run-ios
   ```

Там RNFS / RNFetchBlob будуть підключені як native-модулі, і можна буде реально тестувати методи `readFile('base64')`, `fetch(...).base64()` тощо.

---

## 5) Який метод ми використовуємо в нашому Expo-проєкті

В нашому навчальному проєкті `gallery-client` (Expo) ми залишаємо **робочий метод перетворення зображення в base64** через `axios` (або `fetch`), який не потребує жодних додаткових native-модулів.

Приклад робочого методу:

```ts
// Робочий метод перетворення зображення в base64 у Expo-проєкті
const imageToBase64 = async () => {
    try {
        if (!file) return;

        const response = await axios.get(file, { responseType: 'arraybuffer' });

        const buffer = response.data as ArrayBuffer;
        const bytes = new Uint8Array(buffer);

        const b64 = btoa(
            bytes.reduce((acc, byte) => acc + String.fromCharCode(byte), '')
        );

        setBase64(b64);
    } catch (error) {
        console.log(error);
        setError(error as string);
        return null;
    }
};
```

Таким чином:

- Код у проєкті **залишається чистим і повністю сумісним з Expo Go**.
- Варіанти з `react-native-fs` і `rn-fetch-blob` зберігаються **в цьому файлі `METHOD_ToBase64.md`** як додаткові приклади для майбутніх проєктів (bare React Native або Expo з custom dev client).
