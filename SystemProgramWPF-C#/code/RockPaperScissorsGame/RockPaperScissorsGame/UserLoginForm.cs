using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class UserLoginForm : Form
    {
        public string UserEmail { get; private set; }
        public string UserPassword { get; private set; }

        public UserLoginForm()
        {
            InitializeComponent();
            this.KeyPreview = true; // Дозволяє обробляти натискання клавіш для всієї форми
            this.KeyDown += UserLoginForm_KeyDown; // Додаємо обробник для клавіш
            this.txtPassword.KeyDown += new KeyEventHandler(txtPassword_KeyDown); // Додаємо обробник для TextBox
        }

        private bool ValidateUser(string email, string password)
        {
            if (!File.Exists("accounts.txt"))
            {
                MessageBox.Show("Файл accounts.txt не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var accounts = File.ReadAllLines("accounts.txt");
            bool emailFound = false;

            foreach (var account in accounts)
            {
                var parts = account.Split(',');
                if (parts.Length == 3 && parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    emailFound = true;
                    if (parts[2] == password)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Невірний пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }

            if (!emailFound)
            {
                MessageBox.Show("Користувача з таким email не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Перевірка формату email
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Невірний формат email!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateUser(email, password))
            {
                return;
            }

            // Якщо користувач існує, закриваємо форму з позитивним результатом
            UserEmail = email;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UserLoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); // Виклик функції входу
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick(); // Виклик функції скасування
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); // Виклик функції входу
            }
        }
    }
}