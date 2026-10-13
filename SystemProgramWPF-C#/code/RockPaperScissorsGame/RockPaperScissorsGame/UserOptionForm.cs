using System;
using System.Drawing;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class UserOptionForm : Form
    {
        public bool IsLoginSelected { get; private set; } = false;

        public UserOptionForm()
        {
            InitializeComponent();
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            IsLoginSelected = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            IsLoginSelected = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}