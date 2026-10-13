using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class UserRegistrationForm : Form
    {
        public UserRegistrationForm()
        {
            InitializeComponent();
            this.KeyPreview = true; // Дозволяє перехоплювати натискання клавіш на рівні форми
            this.KeyDown += UserRegistrationForm_KeyDown; // Додаємо обробник подій клавіш
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9]+@[a-zA-Z]+\.[a-zA-Z]{2,}$"))
            {
                MessageBox.Show("Введено некоректний email!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (UserExists(email))
            {
                MessageBox.Show("Користувач з таким email вже існує!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DataManager.AddUser(name, email, password))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Сталася помилка при реєстрації користувача!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool UserExists(string email)
        {
            if (!File.Exists("accounts.txt"))
            {
                return false;
            }

            var accounts = File.ReadAllLines("accounts.txt");
            foreach (var account in accounts)
            {
                var parts = account.Split(',');
                if (parts.Length >= 2 && parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void UserRegistrationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnRegister.PerformClick(); // Емуляція натискання кнопки "Реєстрація"
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel.PerformClick(); // Емуляція натискання кнопки "Скасувати"
            }
        }
    }
}