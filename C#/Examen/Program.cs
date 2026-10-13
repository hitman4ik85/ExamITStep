using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MyGame
{
    // Клас User для даних гравця
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        private string Password { get; set; }

        // Конструктор для створення нового користувача
        public User(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }

        // Метод перевірки введених email та пароля для аутентифікації
        public bool Authenticate(string email, string password) =>
            this.Email == email && this.Password == password;

        // Перевірка, чи вже зареєстрований користувач з таким email
        public static bool IsEmailAlreadyRegistered(string email, List<User> users) =>
            users.Exists(user => user.Email == email);

        // Реєстрація нового гравця: запит імені, email та пароля
        public static User Register(List<User> users, string filePath)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           РЕЄСТРАЦІЯ НОВОГО ГРАВЦЯ           ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.Write("Введіть ім'я: ");
            string name = Console.ReadLine() ?? string.Empty;

            string email;
            int attemptCount = 0;

            while (true)
            {
                Console.Write("Введіть e-mail: ");
                email = Console.ReadLine() ?? string.Empty;

                if (IsEmailAlreadyRegistered(email, users))
                {
                    Console.WriteLine("Такий e-mail вже зареєстрований. Спробуйте інший.");
                    attemptCount++;
                }
                else if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
                {
                    Console.WriteLine("E-mail введено некоректно. Спробуйте ще раз.");
                    attemptCount++;
                }
                else
                {
                    break;
                }

                if (attemptCount >= 3)
                {
                    Console.WriteLine("\nЗанадто багато невдалих спроб введення e-mail.");
                    Console.WriteLine("1. Повторити реєстрацію");
                    Console.WriteLine("2. Повернутися до головного меню");
                    Console.Write("Оберіть дію: ");
                    int retryChoice = Program.ValidateChoice(1, 2);

                    if (retryChoice == 2)
                    {
                        return null;
                    }
                    attemptCount = 0;
                }
            }

            Console.Write("Введіть пароль: ");
            string password = GetPassword();

            User newUser = new User(name, email, password);
            users.Add(newUser);

            Console.WriteLine("\nАкаунт гравця успішно створений.");
            SaveUsersToFile(users, "accaunts.txt");
            Console.WriteLine("Дані акаунтів збережено.");
            return newUser;
        }

        // Метод для введення пароля (прихованим *)
        public static string GetPassword()
        {
            StringBuilder password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Length--;
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
            }
            Console.WriteLine();
            return password.ToString();
        }

        // Метод збереження списку користувачів до файлу
        public static void SaveUsersToFile(List<User> users, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    foreach (var user in users)
                    {
                        writer.WriteLine($"Name: {user.Name}, Email: {user.Email}, Password: {user.Password}");
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Помилка доступу до основного файлу. Спробуйте ще раз пізніше.");
                Console.WriteLine(ex.Message);
            }
        }

        // Метод для завантаження користувачів з файлу у список користувачів
        public static List<User> LoadUsersFromFile(string filePath)
        {
            List<User> users = new List<User>();

            if (File.Exists(filePath))
            {
                try
                {
                    foreach (string line in File.ReadLines(filePath))
                    {
                        string[] parts = line.Split(", ");
                        if (parts.Length >= 3)
                        {
                            string name = parts[0].Split(": ")[1];
                            string email = parts[1].Split(": ")[1];
                            string password = parts[2].Split(": ")[1];
                            users.Add(new User(name, email, password));
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Помилка доступу до основного файлу. Спробуйте ще раз пізніше.");
                    Console.WriteLine(ex.Message);
                }
            }

            return users;
        }

        // Метод для видалення користувача та його статистики
        public static bool DeleteUser(string email, List<User> users, string filePath)
        {
            var userToRemove = users.Find(user => user.Email == email);
            if (userToRemove != null)
            {
                users.Remove(userToRemove);
                Console.WriteLine($"\nГравець з e-mail {email} успішно видалений.");

                string statisticsFilePath = Path.Combine("RezyltatuGru", $"{userToRemove.Email}_statistics.txt");
                if (File.Exists(statisticsFilePath))
                {
                    try
                    {
                        File.Delete(statisticsFilePath);
                        Console.WriteLine("Файл статистики користувача також було видалено.");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Помилка під час видалення файлу статистики. Спробуйте ще раз.");
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Файл статистики користувача не знайдено.");
                }

                SaveUsersToFile(users, filePath);
                return true;
            }
            else
            {
                Console.WriteLine("Користувача з таким e-mail не знайдено.");
                return false;
            }
        }
    }

    // Клас Admin для даних адміністратора
    public class Admin
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        private static readonly string adminFilePath = "admin.txt";

        // Конструктор для створення нового адміністратора з логіном та паролем
        public Admin(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        // Метод для аутентифікації адміністратора
        public bool Authenticate(string username, string password) =>
            this.Username == username && this.Password == password;

        // Завантаження адміністратора з файлу або створення нового
        public static Admin LoadAdmin()
        {
            if (File.Exists(adminFilePath))
            {
                string[] lines = File.ReadAllLines(adminFilePath);
                string username = lines[0].Split(": ")[1];
                string password = lines[1].Split(": ")[1];
                return new Admin(username, password);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(adminFilePath))
                {
                    writer.WriteLine("Login: admin");
                    writer.WriteLine("Password: admin");
                }
                return new Admin("admin", "admin");
            }
        }

        // Оновлення логіна та пароля адміністратора
        public void UpdateCredentials(string newUsername, string newPassword)
        {
            this.Username = newUsername;
            this.Password = newPassword;
            SaveAdmin();
            Console.WriteLine("\nЛогін та пароль адміністратора успішно змінено.");
        }

        // Збереження даних адміністратора у файл
        private void SaveAdmin()
        {
            using (StreamWriter writer = new StreamWriter(adminFilePath, false))
            {
                writer.WriteLine($"Username: {this.Username}");
                writer.WriteLine($"Password: {this.Password}");
            }
        }
    }

    // Клас Game для гри "Камінь, ножиці, папір"
    public class Game
    {
        private static string[] Options = { "Камінь", "Ножиці", "Папір" };

        // Метод очищення статистики гравця
        public static void ClearStatistics(string playerEmail)
        {
            string filePathPlayer = Path.Combine("RezyltatuGru", $"{playerEmail}_statistics.txt");

            if (File.Exists(filePathPlayer))
            {
                File.WriteAllText(filePathPlayer, string.Empty);
                Console.WriteLine($"Статистика гравця з email '{playerEmail}' успішно очищена.");
            }
            else
            {
                Console.WriteLine($"Файл статистики для гравця з email '{playerEmail}' не знайдено.");
            }
        }

        // Гра проти комп'ютера у 3 раунди
        public static void PlayAgainstComputer(User player)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║             ГРА ПРОТИ КОМП'ЮТЕРА             ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            int playerWins = 0;
            int computerWins = 0;

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\nРаунд {i}");
                string playerChoice = GetChoice(player.Name);
                string computerChoice = Options[new Random().Next(0, Options.Length)];
                Console.WriteLine($"\nВибір Комп'ютера: {computerChoice}");

                string roundResult = WhoWonTheRound(player.Name, "Комп'ютер", playerChoice, computerChoice);
                Console.WriteLine($"\n{roundResult}\n");

                if (roundResult.Contains($"гравець '{player.Name}' переміг"))
                {
                    playerWins++;
                }
                else if (roundResult.Contains("гравець 'Комп'ютер' переміг"))
                {
                    computerWins++;
                }
            }

            string result = GenerateGameResult(player.Name, "Комп'ютер", playerWins, computerWins);
            Console.WriteLine(result);
            SaveResult(player.Email, result);
        }

        // Гра проти іншого гравця у 3 раунди
        public static void PlayAgainstPlayer(User player1, User player2)
        {
            Console.WriteLine($"\nГра почалася між {player1.Name} та {player2.Name}!");

            int player1Wins = 0;
            int player2Wins = 0;

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\nРаунд {i}");
                string player1Choice = GetChoice(player1.Name);
                string player2Choice = GetChoice(player2.Name);
                Console.WriteLine($"\n{player1.Name} вибрав: {player1Choice}");
                Console.WriteLine($"{player2.Name} вибрав: {player2Choice}");

                string roundResult = WhoWonTheRound(player1.Name, player2.Name, player1Choice, player2Choice);
                Console.WriteLine($"\n{roundResult}\n");

                if (roundResult.Contains($"гравець '{player1.Name}' переміг"))
                {
                    player1Wins++;
                }
                else if (roundResult.Contains($"гравець '{player2.Name}' переміг"))
                {
                    player2Wins++;
                }
            }

            string result = GenerateGameResult(player1.Name, player2.Name, player1Wins, player2Wins);
            Console.WriteLine(result);

            SaveResult(player1.Email, result);
            SaveResult(player2.Email, result);

            while (true)
            {
                Console.WriteLine("\n1. Бажаєте зіграти ще?");
                Console.WriteLine("2. Повернутися до меню гравця");
                Console.Write("Оберіть пункт меню: ");
                int choice = Program.ValidateChoice(1, 2);

                if (choice == 1)
                {
                    PlayAgainstPlayer(player1, player2);
                    break;
                }
                else if (choice == 2)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                }
            }
        }

        // Метод для вибору ходу гравця
        public static string GetChoice(string playerName)
        {
            Console.WriteLine($"\n{playerName}, Оберіть: 1 - Камінь, 2 - Ножиці, 3 - Папір");

            int choice = -1;
            while (true)
            {
                try
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    choice = Convert.ToInt32(key.KeyChar.ToString());

                    if (choice >= 1 && choice <= 3)
                    {
                        return choice == 1 ? "Камінь" : choice == 2 ? "Ножиці" : "Папір";
                    }
                    else
                    {
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                }
            }
        }

        // Визначення переможця кожного раунду
        private static string WhoWonTheRound(string player1, string player2, string choice1, string choice2)
        {
            if (choice1 == choice2) return "Раунд: Нічия!";
            if ((choice1 == "Камінь" && choice2 == "Ножиці") ||
                (choice1 == "Ножиці" && choice2 == "Папір") ||
                (choice1 == "Папір" && choice2 == "Камінь"))
            {
                return $"Раунд: гравець '{player1}' переміг";
            }
            return $"Раунд: гравець '{player2}' переміг";
        }

        // Форматування підсумкового результату гри в 3 раунди
        private static string GenerateGameResult(string player1, string player2, int player1Wins, int player2Wins)
        {
            string resultTime = DateTime.Now.ToString("ddd dd.MM.yy HH:mm:ss");
            if (player1Wins > player2Wins)
            {
                return $"{resultTime} - '{player1}' грав проти '{player2}' у 3 раунди, '{player1}' переміг";
            }
            else if (player2Wins > player1Wins)
            {
                return $"{resultTime} - '{player1}' грав проти '{player2}' у 3 раунди, '{player2}' переміг";
            }
            else
            {
                return $"{resultTime} - '{player1}' грав проти '{player2}' у 3 раунди, нічия";
            }
        }

        // Збереження результату гри у файл статистики гравця
        private static void SaveResult(string playerEmail, string result) =>
                File.AppendAllText(Path.Combine("RezyltatuGru", $"{playerEmail}_statistics.txt"), result + "\n");
    }

    // Основний клас Program, який об'єднує всі компоненти та логіку гри
    public class Program
    {
        private static string filePath = "accaunts.txt";
        private static List<User> users;
        private static Admin admin;

        public static void Main()
        {
            // Щоб бачити українські літери
            Console.OutputEncoding = Encoding.UTF8;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.InputEncoding = Encoding.GetEncoding("windows-1251");

            users = User.LoadUsersFromFile(filePath);
            admin = Admin.LoadAdmin();

            // Додавання перевірки та створення папки RezyltatuGru
            string statsDirectory = "RezyltatuGru";
            if (!Directory.Exists(statsDirectory))
            {
                try
                {
                    Directory.CreateDirectory(statsDirectory);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Помилка створення папки {statsDirectory}: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Недостатньо прав для створення папки {statsDirectory}: {ex.Message}");
                }
            }

            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║     ВІТАЄМО У ГРІ 'КАМІНЬ, НОЖИЦІ, ПАПІР'    ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════╗");
                Console.WriteLine("║               ГОЛОВНЕ МЕНЮ                   ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.WriteLine("1. Меню адміністратора");
                Console.WriteLine("2. Реєстрація нового гравця");
                Console.WriteLine("3. Вхід до акаунту гравця");
                Console.WriteLine("4. Вихід з програми");
                Console.Write("Оберіть пункт меню: ");
                Console.WriteLine();
                int choice = ValidateChoice(1, 4);

                switch (choice)
                {
                    case 1: AdminMenu(); break;
                    case 2: RegisterUser(); break;
                    case 3: Login(); break;
                    case 4:
                        User.SaveUsersToFile(users, filePath);
                        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
                        Console.WriteLine("║          ДЯКУЄМО ЗА ГРУ! ДО ПОБАЧЕННЯ!       ║");
                        Console.WriteLine("╚══════════════════════════════════════════════╝");
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }

        // Меню адміністратора
        private static void AdminMenu()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           ВХІД ДО МЕНЮ АДМІНІСТРАТОРА        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Console.Write("Введіть логін адміністратора: ");
            string username = Console.ReadLine() ?? string.Empty;
            Console.Write("Введіть пароль адміністратора: ");
            string password = User.GetPassword();

            if (admin.Authenticate(username, password))
            {
                while (true)
                {
                    Console.WriteLine("\n╔══════════════════════════════════════════════╗");
                    Console.WriteLine("║              МЕНЮ АДМІНІСТРАТОРА             ║");
                    Console.WriteLine("╚══════════════════════════════════════════════╝");
                    Console.WriteLine("1. Перегляд всіх гравців");
                    Console.WriteLine("2. Видалення гравця");
                    Console.WriteLine("3. Змінити логін та пароль адміністратора");
                    Console.WriteLine("4. Вихід до головного меню");
                    Console.Write("Оберіть пункт меню: ");
                    Console.WriteLine();
                    int choice = ValidateChoice(1, 4);

                    switch (choice)
                    {
                        case 1:
                            ShowAllUsers();
                            break;
                        case 2:
                            DeleteUser();
                            break;
                        case 3:
                            ChangeAdminLogPass();
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("\nНевірний логін або пароль адміністратора.");
            }
        }

        // Метод для реєстрації нового користувача
        private static void RegisterUser()
        {
            User newUser = User.Register(users, filePath);
            if (newUser != null)
            {
                Console.WriteLine($"{newUser.Name}!, для початку гри здійсніть вхід до акаунту");
            }
        }

        // Метод для входу користувача до системи
        private static void Login()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║             ВХІД ДО АКАУНТУ ГРАВЦЯ           ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            int attemptCount = 0;

            while (true)
            {
                Console.Write("Введіть e-mail: ");
                string email = Console.ReadLine() ?? string.Empty;

                if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
                {
                    Console.WriteLine("E-mail введено некоректно. Спробуйте ще раз.");
                    attemptCount++;
                }
                else
                {
                    Console.Write("Введіть пароль: ");
                    string password = User.GetPassword();

                    var user = users.Find(u => u.Authenticate(email, password));

                    if (user != null)
                    {
                        Console.WriteLine($"\nЛаскаво просимо, {user.Name}!");
                        PlayerMenu(user);
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Невірний e-mail або пароль.");
                        attemptCount++;
                    }
                }

                if (attemptCount >= 3)
                {
                    Console.WriteLine("\nЗанадто багато невдалих спроб входу.");
                    Console.WriteLine("1. Повторити спробу входу");
                    Console.WriteLine("2. Повернутися до головного меню");
                    Console.Write("Оберіть дію: ");
                    int retryChoice = ValidateChoice(1, 2);

                    if (retryChoice == 2)
                    {
                        return;
                    }
                    attemptCount = 0;
                }
            }
        }

        // Виводить список усіх зареєстрованих користувачів
        private static void ShowAllUsers()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║         СПИСОК ЗАРЕЄСТРОВАНИХ ГРАВЦІВ        ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            if (users.Count == 0)
            {
                Console.WriteLine("Немає зареєстрованих гравців.");
            }
            else
            {
                foreach (User user in users)
                {
                    Console.WriteLine($"Ім'я: {user.Name}, E-mail: {user.Email}");
                }
            }
        }

        // Меню гравця після входу
        private static void PlayerMenu(User player)
        {
            while (true)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════╗");
                Console.WriteLine($"║            МЕНЮ ГРАВЦЯ: {player.Name}               ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
                Console.WriteLine("1. Гра проти комп'ютера");
                Console.WriteLine("2. Гра проти іншого гравця");
                Console.WriteLine("3. Перегляд статистики");
                Console.WriteLine("4. Очистити статистику");
                Console.WriteLine("5. Вихід до головного меню");
                Console.Write("Оберіть пункт меню: ");
                Console.WriteLine();
                int choice = ValidateChoice(1, 5);

                switch (choice)
                {
                    case 1:
                        Game.PlayAgainstComputer(player);
                        break;
                    case 2:
                        PlayAgainstAnotherPlayer(player);
                        break;
                    case 3:
                        ShowStatistics(player.Email);
                        break;
                    case 4:
                        Game.ClearStatistics(player.Email);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                        break;
                }
            }
        }

        // Видаляє обраного користувача та його статистику
        private static void DeleteUser()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║               ВИДАЛЕННЯ ГРАВЦЯ               ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.Write("Введіть e-mail гравця для видалення: ");
            string email = Console.ReadLine() ?? string.Empty;

            bool success = User.DeleteUser(email, users, filePath);
            if (success)
            {
                User.SaveUsersToFile(users, filePath);
            }
        }

        // Змінює логін та пароль адміністратора
        private static void ChangeAdminLogPass()
        {
            Console.Write("Введіть новий логін адміністратора: ");
            string newUsername = Console.ReadLine() ?? string.Empty;
            Console.Write("Введіть новий пароль адміністратора: ");
            string newPassword = User.GetPassword();

            if (admin.Username == newUsername)
            {
                Console.WriteLine("\nНовий логін не повинен збігатися з поточним. Будь ласка, введіть інший логін.");
                return;
            }

            admin.UpdateCredentials(newUsername, newPassword);
        }

        // Відображає статистику для обраного гравця
        private static void ShowStatistics(string playerEmail)
        {
            string filePathPlayer = Path.Combine("RezyltatuGru", $"{playerEmail}_statistics.txt");
            Console.WriteLine($"\n╔══════════════════════════════════════════════╗");
            Console.WriteLine($"║      СТАТИСТИКА ЗІГРАНИХ ІГОР ГРАВЦЯ         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            if (File.Exists(filePathPlayer))
            {
                foreach (string line in File.ReadLines(filePathPlayer))
                    Console.WriteLine(line);
            }
            else Console.WriteLine("Статистика поки недоступна.");
        }

        // Перевірка та обробка вибору гравця в меню без натискання Enter
        public static int ValidateChoice(int min, int max)
        {
            int choice = -1;
            while (true)
            {
                try
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    choice = Convert.ToInt32(key.KeyChar.ToString());

                    if (choice >= min && choice <= max)
                    {
                        Console.WriteLine(choice);
                        return choice;
                    }
                    else
                    {
                        Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Невірний вибір, спробуйте ще раз.");
                }
            }
        }

        // Гра проти іншого гравця, з можливістю зареєструвати нового гравця або ввійти в акаунт
        private static void PlayAgainstAnotherPlayer(User player1)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║           ГРА ПРОТИ ІНШОГО ГРАВЦЯ            ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine("1. Створення нового гравця");
            Console.WriteLine("2. Вхід до вже існуючого акаунту");
            Console.Write("Оберіть необхідний пункт меню: ");
            int actionChoice = ValidateChoice(1, 2);

            User player2 = null;
            if (actionChoice == 1)
            {
                player2 = User.Register(users, filePath);
                if (player2 == null)
                {
                    Console.WriteLine("Не вдалося створити другого гравця. Повернення до головного меню.");
                    return;
                }
            }
            else if (actionChoice == 2)
            {
                player2 = LoginForSecondPlayer(player1.Email);
                if (player2 == null)
                {
                    Console.WriteLine("Не вдалося увійти другим гравцем. Повернення до головного меню.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір.");
                return;
            }

            Game.PlayAgainstPlayer(player1, player2);
        }

        // Метод для входу другого гравця, з перевіркою на унікальний email
        private static User LoginForSecondPlayer(string firstPlayerEmail)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║             ВХІД ДО АКАУНТУ ГРАВЦЯ           ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            int attemptCount = 0;

            while (true)
            {
                Console.Write("Введіть e-mail: ");
                string email = Console.ReadLine() ?? string.Empty;

                if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
                {
                    Console.WriteLine("E-mail введено некоректно. Спробуйте ще раз.");
                    attemptCount++;
                }
                else if (email == firstPlayerEmail)
                {
                    Console.WriteLine("Цей e-mail вже використовується першим гравцем. Введіть інший e-mail.");
                    attemptCount++;
                }
                else
                {
                    Console.Write("Введіть пароль: ");
                    string password = User.GetPassword();

                    var user = users.Find(u => u.Authenticate(email, password));

                    if (user != null)
                    {
                        Console.WriteLine($"Ласкаво просимо, {user.Name}!");
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("Невірний e-mail або пароль.");
                        attemptCount++;
                    }
                }

                if (attemptCount >= 3)
                {
                    Console.WriteLine("\nЗанадто багато невдалих спроб входу.");
                    Console.WriteLine("1. Повторити спробу входу");
                    Console.WriteLine("2. Повернутися до головного меню");
                    Console.Write("Оберіть дію: ");

                    int retryChoice = ValidateChoice(1, 2);
                    if (retryChoice == 2)
                    {
                        return null;
                    }
                    attemptCount = 0;
                }
            }
        }
    }
}