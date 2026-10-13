using System;
using System.Drawing;
using System.Windows.Forms;
using WMPLib; // Для відтворення MP3 файлів

namespace RockPaperScissorsGame
{
    public partial class MainForm : Form
    {
        private WindowsMediaPlayer mediaPlayer;
        private bool isMuted = false;

        public MainForm()
        {
            InitializeComponent();
            SetBackgroundImage();
            InitializeBackgroundMusic();
        }

        private void SetBackgroundImage()
        {
            try
            {
                this.BackgroundImage = Image.FromFile("Resources/program_background.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження фону: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeBackgroundMusic()
        {
            try
            {
                mediaPlayer = new WindowsMediaPlayer();
                mediaPlayer.URL = "Resources/sound.mp3";
                mediaPlayer.settings.setMode("loop", true); // Зациклення
                mediaPlayer.controls.play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження музики: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdminMenu_Click(object sender, EventArgs e)
        {
            AdminLoginForm adminLogin = new AdminLoginForm();
            adminLogin.ShowDialog();
        }

        private void btnUserMenu_Click(object sender, EventArgs e)
        {
            UserOptionForm userOptionForm = new UserOptionForm();
            if (userOptionForm.ShowDialog() == DialogResult.OK)
            {
                if (userOptionForm.IsLoginSelected)
                {
                    UserLoginForm loginForm = new UserLoginForm();
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        UserMenuForm userMenuForm = new UserMenuForm(loginForm.UserEmail);
                        userMenuForm.ShowDialog();
                    }
                }
                else
                {
                    UserRegistrationForm registrationForm = new UserRegistrationForm();
                    if (registrationForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Реєстрація успішна! Тепер ви можете увійти.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        UserLoginForm loginForm = new UserLoginForm();
                        if (loginForm.ShowDialog() == DialogResult.OK)
                        {
                            UserMenuForm userMenuForm = new UserMenuForm(loginForm.UserEmail);
                            userMenuForm.ShowDialog();
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void infoMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Гра 'Камінь Ножиці Папір'", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            try
            {
                if (isMuted)
                {
                    mediaPlayer.controls.play(); // Відновлюємо музику
                    btnMute.BackgroundImage = Image.FromFile("Resources/mute.jpg");
                }
                else
                {
                    mediaPlayer.controls.pause(); // Зупиняємо музику
                    btnMute.BackgroundImage = Image.FromFile("Resources/unmute.jpg");
                }
                isMuted = !isMuted;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка керування музикою: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}