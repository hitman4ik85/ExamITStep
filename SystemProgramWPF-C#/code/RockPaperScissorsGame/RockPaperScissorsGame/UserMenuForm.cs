using System;
using System.Drawing;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class UserMenuForm : Form
    {
        private string userEmail;

        public UserMenuForm(string email)
        {
            InitializeComponent();
            userEmail = email;
            SetBackgroundImage();
        }

        private void SetBackgroundImage()
        {
            try
            {
                this.BackgroundImage = Image.FromFile("Resources/player_menu.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження фону: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPlayWithComputer_Click(object sender, EventArgs e)
        {
            GameWithComputerForm gameWithComputer = new GameWithComputerForm(userEmail);
            gameWithComputer.ShowDialog();
        }

        private void btnPlayWithPlayer_Click(object sender, EventArgs e)
        {
            GameWithPlayerForm gameWithPlayer = new GameWithPlayerForm(userEmail);
            gameWithPlayer.ShowDialog();
        }

        private void btnViewStatistics_Click(object sender, EventArgs e)
        {
            StatisticsForm statistics = new StatisticsForm(userEmail);
            statistics.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangeUserData_Click(object sender, EventArgs e)
        {
            ChangeUserCredentialsForm changeForm = new ChangeUserCredentialsForm(userEmail);
            if (changeForm.ShowDialog() == DialogResult.OK)
            {
                // Оновлюємо e-mail після зміни даних
                userEmail = changeForm.UpdatedEmail;

                // Перезавантаження форми
                this.Hide();
                UserMenuForm updatedMenu = new UserMenuForm(userEmail);
                updatedMenu.ShowDialog();
                this.Close();
            }
        }
    }
}