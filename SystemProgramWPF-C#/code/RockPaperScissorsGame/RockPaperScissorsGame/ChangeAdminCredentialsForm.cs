using System;
using System.IO;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class ChangeAdminCredentialsForm : Form
    {
        private const string AdminFilePath = "admin.txt";

        public ChangeAdminCredentialsForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += ChangeAdminCredentialsForm_KeyDown;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newUsername = txtNewEmail.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Паролі не співпадають!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (File.Exists(AdminFilePath))
                {
                    // Зчитуємо поточні дані адміністратора
                    var adminData = File.ReadAllLines(AdminFilePath);
                    if (adminData.Length >= 2)
                    {
                        string currentLogin = adminData[0].Split(':')[1].Trim();
                        string currentPassword = adminData[1].Split(':')[1].Trim();

                        // Перевірка, чи не збігаються нові дані з поточними
                        if (newUsername == currentLogin && newPassword == currentPassword)
                        {
                            MessageBox.Show("Неможливо змінити дані на такі ж самі!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Оновлюємо дані адміністратора
                    File.WriteAllText(AdminFilePath, $"Login: {newUsername}\nPassword: {newPassword}");
                    MessageBox.Show("Облікові дані адміністратора успішно змінено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Файл облікових записів не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangeAdminCredentialsForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
    }
}