using System;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class AdminMenuForm : Form
    {
        public AdminMenuForm()
        {
            InitializeComponent();
            SetBackgroundImage();
        }

        private void SetBackgroundImage()
        {
            try
            {
                this.BackgroundImage = Image.FromFile("Resources/admin_menu.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження фону: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewPlayers_Click(object sender, EventArgs e)
        {
            PlayerManagementForm playerManagement = new PlayerManagementForm();
            playerManagement.ShowDialog();
        }

        private void btnChangeAdminCredentials_Click(object sender, EventArgs e)
        {
            ChangeAdminCredentialsForm changeAdminCredentials = new ChangeAdminCredentialsForm();
            changeAdminCredentials.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}