using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public static class DataManager
    {
        private const string AccountsFilePath = "accounts.txt";
        private const string StatisticsFolderPath = "statistics";

        static DataManager()
        {
            if (!Directory.Exists(StatisticsFolderPath))
            {
                Directory.CreateDirectory(StatisticsFolderPath);
            }
        }

        public static bool AddUser(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (IsUserRegistered(email))
            {
                return false;
            }

            File.AppendAllText(AccountsFilePath, $"{name},{email},{password}\n");
            return true;
        }

        public static bool IsUserRegistered(string email)
        {
            if (!File.Exists(AccountsFilePath))
            {
                return false;
            }

            var accounts = File.ReadAllLines(AccountsFilePath);
            foreach (var account in accounts)
            {
                var parts = account.Split(',');
                if (parts.Length > 1 && parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetUserName(string? email)
        {
            if (email == "Комп'ютер")
            {
                return "Комп'ютер";
            }

            if (string.IsNullOrEmpty(email))
            {
                return "Невідомий гравець";
            }

            if (File.Exists(AccountsFilePath))
            {
                var accounts = File.ReadAllLines(AccountsFilePath);
                foreach (var account in accounts)
                {
                    var parts = account.Split(',');
                    if (parts.Length > 1 && parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                    {
                        return parts[0]; // Повертаємо ім'я
                    }
                }
            }

            return "Невідомий гравець";
        }

        public static void AddGameResult(string userEmail, string opponentName, string result, string playerChoice, string opponentChoice)
        {
            try
            {
                string filePath = Path.Combine(StatisticsFolderPath, $"{userEmail}.statistic.txt");

                if (!Directory.Exists(StatisticsFolderPath))
                {
                    Directory.CreateDirectory(StatisticsFolderPath);
                }

                // Ім'я гравця
                string userName = GetUserName(userEmail);

                // Якщо супротивник — комп'ютер, залишаємо "Комп'ютер"
                string opponentDisplayName = opponentName == "Комп'ютер" ? "Комп'ютер" : GetUserName(opponentName);

                // Формуємо запис
                string logEntry = $"{DateTime.Now}: {userName} грав проти {opponentDisplayName}, Ваш хід: {playerChoice}, Хід супротивника: {opponentChoice}, результат: {result}";

                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка запису статистики: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static List<string> GetStatistics(string email)
        {
            var results = new List<string>();
            string filePath = Path.Combine(StatisticsFolderPath, $"{email}.statistic.txt");

            if (File.Exists(filePath))
            {
                results.AddRange(File.ReadAllLines(filePath));
            }

            return results;
        }

        public static void ClearStatistics(string email)
        {
            try
            {
                string statisticsFilePath = Path.Combine(StatisticsFolderPath, $"{email}.statistic.txt");

                if (File.Exists(statisticsFilePath))
                {
                    File.Delete(statisticsFilePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні статистики для {email}: {ex.Message}");
            }
        }

        public static void RemoveUser(string email)
        {
            // Удаляем пользователя из accounts.txt
            if (File.Exists(AccountsFilePath))
            {
                var accounts = File.ReadAllLines(AccountsFilePath);
                var filteredAccounts = new List<string>();

                foreach (var account in accounts)
                {
                    var parts = account.Split(',');
                    if (parts.Length > 1 && !parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredAccounts.Add(account);
                    }
                }

                File.WriteAllLines(AccountsFilePath, filteredAccounts);
            }

            // Удаляем статистику пользователя
            ClearStatistics(email);
        }
    }
}