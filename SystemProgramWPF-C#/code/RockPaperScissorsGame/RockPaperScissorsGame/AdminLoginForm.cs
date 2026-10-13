using System;
using System.IO;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class AdminLoginForm : Form
    {
        private const string AdminFilePath = "admin.txt";

        public AdminLoginForm()
        {
            InitializeComponent();
            EnsureAdminFileExists();
            this.KeyPreview = true; // Натискання клавіш для всієї форми
            this.KeyDown += AdminLoginForm_KeyDown; // Обробник для клавіш

            // Додаємо підказки до текстових полів
            AddToolTips();
        }

        private void EnsureAdminFileExists()
        {
            if (!File.Exists(AdminFilePath))
            {
                File.WriteAllText(AdminFilePath, "Login: admin\nPassword: admin");
            }
        }

        private void AddToolTips()
        {
            var toolTip = new ToolTip();
            toolTip.SetToolTip(txtUsername, "За замовчуванням Логін: admin");
            toolTip.SetToolTip(txtPassword, "За замовчуванням Пароль: admin");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            try
            {
                if (File.Exists(AdminFilePath))
                {
                    var adminData = File.ReadAllLines(AdminFilePath);
                    if (adminData.Length < 2)
                    {
                        MessageBox.Show("Файл даних адміністратора пошкоджений!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string savedUsername = adminData[0].Split(':')[1].Trim();
                    string savedPassword = adminData[1].Split(':')[1].Trim();

                    if (username == savedUsername && password == savedPassword)
                    {
                        AdminMenuForm adminMenu = new AdminMenuForm();
                        adminMenu.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Невірний логін або пароль!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Файл адміністратора не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AdminLoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(sender, e);
            }
        }
    }
}