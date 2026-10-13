using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class ChangeUserCredentialsForm : Form
    {
        private readonly string currentUserEmail;

        public string UpdatedEmail { get; private set; } // Властивість для передачі нового e-mail

        public ChangeUserCredentialsForm(string email)
        {
            InitializeComponent();
            currentUserEmail = email;

            // Дозволяємо перехоплювати натискання клавіш для всієї форми
            this.KeyPreview = true;
            this.KeyDown += ChangeUserCredentialsForm_KeyDown;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newName = txtName.Text.Trim();
            string newEmail = txtEmail.Text.Trim();
            string newPassword = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(newName) || string.IsNullOrWhiteSpace(newEmail) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Усі поля повинні бути заповнені!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (File.Exists("accounts.txt"))
                {
                    var accounts = File.ReadAllLines("accounts.txt").ToList();
                    int index = accounts.FindIndex(a => a.Contains(currentUserEmail));
                    if (index != -1)
                    {
                        // Отримуємо поточний e-mail для перевірки
                        var currentAccountParts = accounts[index].Split(',');
                        string oldEmail = currentAccountParts[1].Trim();

                        // Оновлюємо дані користувача
                        accounts[index] = $"{newName},{newEmail},{newPassword}";
                        File.WriteAllLines("accounts.txt", accounts);

                        // Перевіряємо, чи змінено e-mail
                        if (!oldEmail.Equals(newEmail, StringComparison.OrdinalIgnoreCase))
                        {
                            // Шлях до старого файлу статистики
                            string oldStatisticsPath = Path.Combine("statistics", $"{oldEmail}.statistic.txt");
                            string newStatisticsPath = Path.Combine("statistics", $"{newEmail}.statistic.txt");

                            // Перейменовуємо файл статистики
                            if (File.Exists(oldStatisticsPath))
                            {
                                File.Move(oldStatisticsPath, newStatisticsPath);
                            }
                        }

                        UpdatedEmail = newEmail; // Передаємо оновлений e-mail
                        MessageBox.Show("Дані успішно змінено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Користувача не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Файл облікових записів не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangeUserCredentialsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.PerformClick(); // Виклик функції збереження змін
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick(); // Виклик функції скасування
            }
        }
    }
}