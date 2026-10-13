#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <fstream>
#include <string>
#include <ctime>
#include <vector>
#include <conio.h>
#include <windows.h>
#include <limits>
#include <iomanip>
#include <cstdlib>
using namespace std;

struct TestResult { //структура, для зберігання результатів тесту
    char generatedChar; //зберігає згенерований символ
    char userInput; //введений символ користувачем
    double timeTaken; //час за який користувач ввів символ
};

class User {
public:
    string surname;
    string name;
    string email;
    string password;

    User() = default; //конструктор за замовчуванням
    User(string s, string n, string e, string p) : surname(s), name(n), email(e), password(p) {} //з параметрами

    void changePassword(string newPassword) { //дозволя змінити пароль
        password = newPassword;
    }

    string getFullName() { //поверта імя та прізвище
        return surname + " " + name;
    }

    bool operator==(User& other) { //порівняння об'єктів
        return surname == other.surname && name == other.name && email == other.email && password == other.password;
    }
};
//Оголошення функцій:
void showMenu(); //показує меню програми
void createUser(); //створення нового користувача
void loginUser(); //для авторизації користувача
void changePassword(User& user); //для зміни паролю користувача
void showStatistics(User& user); //для показу статистики користувача
void adminLogin(); //для входу як адміністратор
void adminMenu(); //для меню адміністратора
void showUserList(); //для відображення користувачив в списку
void deleteUser(); //для видалення користувача адміністратором
void writeResultToFile(string fileName, User& user, vector<TestResult>& results); //для запису результатів тесту у файл
char generateRandomChar(int difficulty); //для генерації випадкового символу відповідно до рівня складності
double getTimeTaken(clock_t start, clock_t end); //для розрахунку часу між двома моментами часу началом і кінцем
void runTest(User& user, int difficulty); //для проведення тесту для користувача 
void displayStatistics(User& user, string fileName); //для відображення статистики користувача
bool loadUser(string surname, string name, string email, string password, User& user); //для завантаження даних користувача з файлу 
void saveUser(User& user); //для збереження даних користувача у файл
void deleteUserFromFile(string surname, string name, string email); //для видалення даних користувача з файлу
void deleteUserResults(string surname, string name, string email); //для видалення результатів тесту користувача з файлу

void showMenu() {
    int choice; //зберіга вибір користувача
    string input; //для зчитування вводу користувача

    while (true) {
        cout << "_________________________________";
        cout << "\nМеню програми:\n";
        cout << "1. Створити користувача\n";
        cout << "2. Авторизуватись\n";
        cout << "3. Вхід як адміністратор\n";
        cout << "4. Вихід з програми\n";
        cout << "Зробіть ваш вибір: ";

        getline(cin, input); //зчитування рядка з вводу користувача

        try {
            choice = stoi(input); //перетворення рядка в циле число
        }
        catch (invalid_argument&) {
            cout << "Невірний ввід. Будь ласка, введіть число від 1 до 4." << endl;
            continue;
        }

        if (choice >= 1 && choice <= 4) {
            break;
        }
        else {
            cout << "Невірний вибір. Спробуйте ще раз (1 - 4)." << endl;
        }
    }
    switch (choice) {
    case 1:
        createUser();
        break;
    case 2:
        loginUser();
        break;
    case 3:
        adminLogin();
        break;
    case 4:
        exit(0);
        break;
    }
}

void createUser() { //новий користувач
    string surname, name, email, password;

    cout << "_________________________________\n";
    cout << "Введіть прізвище: ";
    getline(cin, surname);
    cout << "Введіть ім'я: ";
    getline(cin, name);
    cout << "Введіть e-mail: ";
    getline(cin, email);

    cout << "Введіть пароль: ";
    char ch;
    while ((ch = _getch()) != '\r') { //зчитування паролю, доки не натиснути enter
        if (ch == '\b') {             //видалення останнього символу(корекція)
            if (!password.empty()) {
                password.pop_back();
                cout << "\b \b";
            }
        }
        else {
            password += ch;
            cout << '*';
        }
    }
    cout << endl;

    User newUser(surname, name, email, password); //створення користувача

    if (loadUser(surname, name, email, password, newUser)) {
        cout << "Користувач з таким прізвищем, ім'ям та e-mail вже існує." << endl;
        return createUser();
    }
    else {
        saveUser(newUser);
        cout << "Користувача створено успішно." << endl;
        return showMenu();
    }
}

void loginUser() {
    string surname, name, email, password;

    cout << "_________________________________\n";
    cout << "Введіть прізвище: ";
    getline(cin, surname);
    cout << "Введіть ім'я: ";
    getline(cin, name);
    cout << "Введіть e-mail: ";
    getline(cin, email);

    cout << "Введіть пароль: ";
    char ch;
    while ((ch = _getch()) != '\r') {
        if (ch == '\b') {
            if (!password.empty()) {
                password.pop_back();
                cout << "\b \b";
            }
        }
        else {
            password += ch;
            cout << '*';
        }
    }
    cout << endl;

    User user;
    if (loadUser(surname, name, email, password, user)) {
        cout << "Авторизація пройшла успішно. Вітаємо, " << user.getFullName() << "!" << endl;
        cout << "_________________________________\n";
        cout << "Меню користувача:";

        int choice;
        while (true) {
            cout << "\n1. Пройти тестування" << endl;
            cout << "2. Показати статистику" << endl;
            cout << "3. Змінити пароль" << endl;
            cout << "4. Повернутися до головного меню" << endl;
            cout << "Зробіть ваш вибір: ";

            string input;
            getline(cin, input);

            try {
                choice = stoi(input);
            }
            catch (...) {
                cout << "Невірний вибір. Спробуйте ще раз." << endl;
                continue;
            }

            switch (choice) {
            case 1:
                int difficulty;
                while (true) {
                    cout << "____________________________________________________________________" << endl;
                    cout << "Оберіть рівень складності (1 - простий, 2 - середній, 3 - складний): ";
                    getline(cin, input);

                    try {
                        difficulty = stoi(input);
                        if (difficulty < 1 || difficulty > 3) {
                            cout << "Невірний рівень складності. Спробуйте ще раз." << endl;
                        }
                        else {
                            break;
                        }
                    }
                    catch (...) {
                        cout << "Невірний вибір. Спробуйте ще раз." << endl;
                    }
                }
                runTest(user, difficulty);
                break;
            case 2:
                showStatistics(user);
                break;
            case 3:
                changePassword(user);
                break;
            case 4:
                return showMenu();
            default:
                cout << "Невірний вибір. Спробуйте ще раз." << endl;
            }
        }
    }
    else {
        cout << "Користувача не знайдено або невірний пароль." << endl;
        showMenu();
    }
}

void changePassword(User& user) {
    if (user.getFullName().empty()) {
        cout << "Спочатку авторизуйтесь." << endl;
        return;
    }

    string oldPassword, newPassword;
    bool passwordChanged = false;

    cout << "_________________________________\n";
    do {
        cout << "Введіть старий пароль: ";
        oldPassword.clear();

        char ch;
        while ((ch = _getch()) != '\r') {
            if (ch == '\b') {
                if (!oldPassword.empty()) {
                    oldPassword.pop_back();
                    cout << "\b \b";
                }
            }
            else {
                oldPassword += ch;
                cout << '*';
            }
        }
        cout << endl;

        if (oldPassword != user.password) {
            cout << "Невірний старий пароль." << endl;
        }
        else {
            string newPasswordConfirmed;

            do {
                cout << "Введіть новий пароль: ";
                newPassword.clear();

                while ((ch = _getch()) != '\r') {
                    if (ch == '\b') {
                        if (!newPassword.empty()) {
                            newPassword.pop_back();
                            cout << "\b \b";
                        }
                    }
                    else {
                        newPassword += ch;
                        cout << '*';
                    }
                }
                cout << endl;

                if (newPassword == user.password) {
                    cout << "Новий пароль не повинен співпадати зі старим." << endl;
                }
                else {
                    cout << "Підтвердження нового пароля: ";
                    newPasswordConfirmed.clear();

                    while ((ch = _getch()) != '\r') {
                        if (ch == '\b') {
                            if (!newPasswordConfirmed.empty()) {
                                newPasswordConfirmed.pop_back();
                                cout << "\b \b";
                            }
                        }
                        else {
                            newPasswordConfirmed += ch;
                            cout << '*';
                        }
                    }
                    cout << endl;

                    if (newPassword != newPasswordConfirmed) {
                        cout << "Паролі не співпадають. Спробуйте ще раз." << endl;
                    }
                    else {
                        passwordChanged = true;
                        user.changePassword(newPassword);
                        saveUser(user);
                        cout << "Пароль змінено успішно." << endl;
                    }
                }
            } while (!passwordChanged);
        }
    } while (!passwordChanged);

    cout << "_________________________________\n";
    cout << "Зробіть Ваш вибір:" << endl;
}

bool loadUser(string surname, string name, string email, string password, User& user) {
    ifstream file("user.txt");
    if (!file.is_open()) return false;

    string fileSurname, fileName, fileEmail, filePassword;
    while (file >> fileSurname >> fileName >> fileEmail >> filePassword) {
        if (fileSurname == surname && fileName == name && fileEmail == email && filePassword == password) {
            user = User(fileSurname, fileName, fileEmail, filePassword);
            file.close();
            return true;
        }
    }
    file.close();
    return false;
}

void saveUser(User& user) {
    ofstream file("user.txt", ios::app);
    if (file.is_open()) {
        file << user.surname << " " << user.name << " " << user.email << " " << user.password << endl;
        file.close();
    }
}

void runTest(User& user, int difficulty) {
    vector<TestResult> results;

    cout << "____________________________________________________________________" << endl;
    cout << "Тестування починається!" << endl;
    cout << "Рівень складності: " << difficulty << endl;

    char generatedChar;
    char userInput;
    double timeTaken;
    int numberOfTests = 10;

    for (int i = 0; i < numberOfTests; i++) {
        generatedChar = generateRandomChar(difficulty);
        cout << "Введіть символ: " << generatedChar << endl;

        clock_t start = clock();
        userInput = _getch();
        clock_t end = clock();

        timeTaken = getTimeTaken(start, end);
        results.push_back({ generatedChar, userInput, timeTaken });
    }

    string fileName;
    switch (difficulty) {
    case 1:
        fileName = "test1riv.txt";
        break;
    case 2:
        fileName = "test2riv.txt";
        break;
    case 3:
        fileName = "test3riv.txt";
        break;
    }

    writeResultToFile(fileName, user, results);
    cout << "Тестування завершено." << endl;
    cout << "_________________________________";
    cout << "\nМеню користувача:";
}

char generateRandomChar(int difficulty) {
    vector<string> simpleSet = { "abcdefg", "hijklmn", "opqrstu", "vwxyzab", "cdefghi", "ijklmnop", "qrstuvw", "xyzabcd", "efghijk", "lmnopqr" };
    vector<string> mediumSet = { "hello world", "openai chatbot", "code typing", "quick brown fox", "jumps over lazy", "dogs and cats", "c++ programming", "random example", "keyboard input", "fast typing" };
    vector<string> hardSet = { "h3ll0_W0r1d!", "123_ABC_def!", "!@#$%^&*()", "r4nd0m_str1ng", "t3st_c@se_1", "C++_is_fun!", "c0d3_w1th_u5", "t1m3_to_t3st", "try_y0ur_b3st", "g00d_luck!" };

    string characters;
    switch (difficulty) {
    case 1:
        characters = simpleSet[rand() % simpleSet.size()];
        break;
    case 2:
        characters = mediumSet[rand() % mediumSet.size()];
        break;
    case 3:
        characters = hardSet[rand() % hardSet.size()];
        break;
    default:
        characters = simpleSet[0];
    }

    return characters[rand() % characters.length()];
}

double getTimeTaken(clock_t start, clock_t end) {
    return double(end - start) / CLOCKS_PER_SEC;
}

void writeResultToFile(string fileName, User& user, vector<TestResult>& results) {
    ofstream file(fileName, ios::trunc);
    if (file.is_open()) {
        file << "Користувач: " << user.surname << " " << user.name << " " << user.email << endl;
        for (auto& result : results) {
            file << "Згенеровано: " << result.generatedChar << " Введено: " << result.userInput << " Час: " << fixed << setprecision(3) << result.timeTaken << " сек." << endl;
        }
        file.close();
    }
}

void showStatistics(User& user) {
    int difficulty;
    while (true) {
        cout << "____________________________________________________________________\n";
        cout << "Оберіть рівень складності (1 - простий, 2 - середній, 3 - складний): ";
        string input;
        getline(cin, input);
        cout << "____________________________________________________________________\n";

        try {
            difficulty = stoi(input);
            if (difficulty < 1 || difficulty > 3) {
                cout << "Невірний рівень складності. Спробуйте ще раз." << endl;
            }
            else {
                break;
            }
        }
        catch (...) {
            cout << "Невірний вибір. Спробуйте ще раз." << endl;
        }
    }

    string fileName = "test" + to_string(difficulty) + "riv.txt";
    displayStatistics(user, fileName);
}

void displayStatistics(User& user, string fileName) {
    ifstream file(fileName);
    if (!file.is_open()) {
        cout << "Не вдалося відкрити файл для читання статистики." << endl;
        return;
    }

    vector<TestResult> results;
    string line;
    bool userFound = false;
    string userIdentifier = user.getFullName() + " " + user.email;

    while (getline(file, line)) {
        if (line.find("Користувач: " + userIdentifier) != string::npos) {
            userFound = true;
            continue;
        }

        if (userFound) {
            if (line.empty()) {
                userFound = false;
                continue;
            }

            TestResult result;
            int n = sscanf(line.c_str(), "Згенеровано: %c Введено: %c Час: %lf сек.",
                &result.generatedChar, &result.userInput, &result.timeTaken);

            if (n == 3) {
                results.push_back(result);
            }
        }
    }

    file.close();

    if (results.empty()) {
        cout << "Не знайдено жодних результатів для відображення." << endl;
        return;
    }

    int correctCount = 0;
    double totalCorrectTime = 0.0, totalIncorrectTime = 0.0;
    int fastestIndex = 0, slowestIndex = 0;

    for (size_t i = 0; i < results.size(); i++) {
        if (results[i].userInput == results[i].generatedChar) {
            correctCount++;
            totalCorrectTime += results[i].timeTaken;
        }
        else {
            totalIncorrectTime += results[i].timeTaken;
        }

        if (results[i].timeTaken < results[fastestIndex].timeTaken) {
            fastestIndex = i;
        }

        if (results[i].timeTaken > results[slowestIndex].timeTaken) {
            slowestIndex = i;
        }
    }

    double accuracy = (static_cast<double>(correctCount) / results.size()) * 100;
    double averageCorrectTime = correctCount ? totalCorrectTime / correctCount : 0.0;
    double averageIncorrectTime = (results.size() - correctCount) ? totalIncorrectTime / (results.size() - correctCount) : 0.0;

    cout << fixed << setprecision(2);
    cout << "Ваша точність проходження: " << accuracy << "%" << endl;
    cout << "Символ, який був натиснутий найшвидше: " << results[fastestIndex].generatedChar << " (" << results[fastestIndex].timeTaken << " секунд)" << endl;
    cout << "Символ, який був натиснутий найповільніше: " << results[slowestIndex].generatedChar << " (" << results[slowestIndex].timeTaken << " секунд)" << endl;
    cout << "Середній час натискання коректних символів: " << averageCorrectTime << " секунд" << endl;
    cout << "Середній час натискання некоректних символів: " << averageIncorrectTime << " секунд" << endl;
    cout << "____________________________________________________________________\n";
    cout << "Статистика по тесту відображена, зробіть Ваш вибір:";
}

void adminLogin() {
    string adminPassword;
    cout << "_________________________________\n";
    cout << "Введіть пароль адміністратора: ";
    char ch;
    while ((ch = _getch()) != '\r') {  // Чекаємо на натискання клавіші Enter (код '\r')
        if (ch == '\b') {  // Якщо натиснута клавіша Backspace
            if (!adminPassword.empty()) {
                adminPassword.pop_back();  // Видаляємо останній символ з пароля
                cout << "\b \b";  // Видаляємо зірочку на екрані
            }
        }
        else {
            adminPassword += ch;  // Додаємо символ до пароля
            cout << '*';  // Виводимо зірочку на екран
        }
    }
    cout << endl;

    if (adminPassword == "admin") {  // Перевірка пароля адміністратора
        cout << "Вітаємо, Адміністратор!" << endl;
        adminMenu();  // Виклик функції меню адміністратора
    }
    else {
        cout << "Невірний пароль адміністратора." << endl;
        showMenu();  // Повернення до головного меню
    }
}

void adminMenu() {
    int choice;
    while (true) {
        cout << "_________________________________\n";
        cout << "Меню адміністратора:" << endl;
        cout << "1. Показати список користувачів" << endl;
        cout << "2. Видалити користувача" << endl;
        cout << "3. Повернутися до головного меню" << endl;
        cout << "Зробіть ваш вибір: ";

        string input;
        getline(cin, input);

        try {
            choice = stoi(input);  // Перетворюємо ввід у число
        }
        catch (...) {
            cout << "Невірний вибір. Спробуйте ще раз." << endl;
            continue;
        }

        switch (choice) {
        case 1:
            showUserList();
            break;
        case 2:
            deleteUser();
            break;
        case 3:
            showMenu();
            return;
        default:
            cout << "Невірний вибір. Спробуйте ще раз." << endl;
        }
    }
}

void showUserList() {
    ifstream file("user.txt");

    if (!file.is_open()) {
        cout << "Не вдалося відкрити файл з користувачами." << endl;
        return;
    }

    cout << "_________________________________\n";
    cout << "Список користувачів:" << endl;

    string line;
    while (getline(file, line)) {
        cout << line << endl;
    }

    file.close();
}

void deleteUser() {
    string surname, name, email;
    cout << "_________________________________\n";
    cout << "Введіть прізвище користувача для видалення: ";
    getline(cin, surname);
    cout << "Введіть ім'я користувача для видалення: ";
    getline(cin, name);
    cout << "Введіть e-mail користувача для видалення: ";
    getline(cin, email);

    deleteUserFromFile(surname, name, email);
    deleteUserResults(surname, name, email);
    cout << "Користувача та його результати тестування видалено." << endl;
    showMenu();
}

void deleteUserFromFile(string surname, string name, string email) {
    ifstream file("user.txt");
    if (!file.is_open()) {
        cout << "Не вдалося відкрити файл user.txt" << endl;
        return;
    }

    vector<User> users;
    string fileSurname, fileName, fileEmail, filePassword;

    while (file >> fileSurname >> fileName >> fileEmail >> filePassword) {
        if (!(fileSurname == surname && fileName == name && fileEmail == email)) {
            users.emplace_back(fileSurname, fileName, fileEmail, filePassword);
        }
    }
    file.close();

    ofstream outFile("user.txt", ios::trunc);
    if (!outFile.is_open()) {
        cout << "Не вдалося відкрити файл user.txt для запису" << endl;
        return;
    }

    for (const auto& u : users) {
        outFile << u.surname << " " << u.name << " " << u.email << " " << u.password << endl;
    }
    outFile.close();
}

void deleteUserResults(string surname, string name, string email) {
    string fileNames[] = { "test1riv.txt", "test2riv.txt", "test3riv.txt" };

    for (const auto& fileName : fileNames) {
        ifstream file(fileName);
        if (!file.is_open()) {
            cout << "Не вдалося відкрити файл " << fileName << endl;
            continue;
        }

        vector<string> lines;
        string line;
        string userIdentifier = surname + " " + name + " " + email;

        while (getline(file, line)) {
            if (line.find("Користувач: " + userIdentifier) == string::npos) {
                lines.push_back(line);
            }
            else {
                while (getline(file, line) && !line.empty()) {
                }
            }
        }
        file.close();

        ofstream outFile(fileName, ios::trunc);
        if (!outFile.is_open()) {
            cout << "Не вдалося відкрити файл " << fileName << " для запису" << endl;
            continue;
        }

        for (const auto& l : lines) {
            outFile << l << endl;
        }
        outFile.close();
    }
}

int main() {
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);
    srand((unsigned)time(NULL));

    cout << "Вітаємо у програмі:\n";
    cout << "Я у мами піаніст (клавіатурний тренажер)\n";

    showMenu();
    return 0;
}