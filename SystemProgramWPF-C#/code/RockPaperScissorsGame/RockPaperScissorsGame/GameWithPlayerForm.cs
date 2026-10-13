using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class GameWithPlayerForm : Form
    {
        private string player1Email;
        private string? player2Email = null;
        private string? player1Choice = null;
        private string? player2Choice = null;
        private string? temporaryPlayer1Choice = null; // Тимчасовий вибір для Гравця 1
        private string? temporaryPlayer2Choice = null; // Тимчасовий вибір для Гравця 2

        public GameWithPlayerForm(string email)
        {
            InitializeComponent();
            player1Email = email ?? throw new ArgumentNullException(nameof(email));
            SetBackgroundImage();
            LoadButtonImages();
            UpdatePlayerLabels();

            player1PictureBox.Image = Image.FromFile("Resources/player.jpg");
            player2PictureBox.Image = Image.FromFile("Resources/player.jpg");
        }

        private void SetBackgroundImage()
        {
            try
            {
                this.BackgroundImage = Image.FromFile("Resources/game_background.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження фону: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadButtonImages()
        {
            try
            {
                btnRock.Image = ResizeImage("Resources/rock.jpg", btnRock.Width, btnRock.Height);
                btnPaper.Image = ResizeImage("Resources/paper.jpg", btnPaper.Width, btnPaper.Height);
                btnScissors.Image = ResizeImage("Resources/scissors.jpg", btnScissors.Width, btnScissors.Height);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження зображень для кнопок: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Image ResizeImage(string filePath, int width, int height)
        {
            using (var originalImage = Image.FromFile(filePath))
            {
                var resizedImage = new Bitmap(originalImage, new Size(width, height));
                return resizedImage;
            }
        }

        private void UpdatePlayerLabels()
        {
            lblPlayer1.Text = GetUserName(player1Email);
            lblPlayer2.Text = string.IsNullOrEmpty(player2Email) ? "Невідомо" : GetUserName(player2Email);
        }

        private string GetUserName(string? email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return "Невідомо";
            }

            if (File.Exists("accounts.txt"))
            {
                var accounts = File.ReadAllLines("accounts.txt");
                foreach (var account in accounts)
                {
                    var parts = account.Split(',');
                    if (parts.Length == 3 && parts[1].Equals(email, StringComparison.OrdinalIgnoreCase))
                    {
                        return parts[0];
                    }
                }
            }
            return "Невідомо";
        }

        private void btnPlayer2Login_Click(object sender, EventArgs e)
        {
            UserOptionForm optionForm = new UserOptionForm();
            if (optionForm.ShowDialog() == DialogResult.OK)
            {
                if (optionForm.IsLoginSelected)
                {
                    UserLoginForm loginForm = new UserLoginForm();
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        if (loginForm.UserEmail.Equals(player1Email, StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Гравець 2 не може використовувати ті самі дані, що і Гравець 1!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        player2Email = loginForm.UserEmail;
                        UpdatePlayerLabels();
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
                            if (loginForm.UserEmail.Equals(player1Email, StringComparison.OrdinalIgnoreCase))
                            {
                                MessageBox.Show("Гравець 2 не може використовувати ті самі дані, що і Гравець 1!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            player2Email = loginForm.UserEmail;
                            UpdatePlayerLabels();
                        }
                    }
                }
            }
        }

        private void btnPlayer1Choice_Click(object sender, EventArgs e)
        {
            lblMessagePlayer1.Text = "Зробіть хід";
            EnableChoiceButtons();
            DisablePlayerButtons();
            temporaryPlayer1Choice = null;
        }

        private void btnPlayer2Choice_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(player2Email))
            {
                MessageBox.Show("Гравець 2 ще не увійшов у гру!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblMessagePlayer2.Text = "Зробіть хід";
            EnableChoiceButtons();
            DisablePlayerButtons();
            temporaryPlayer2Choice = null;
        }

        private void ChoiceButton_Click(object sender, EventArgs e)
        {
            Button choiceButton = sender as Button;
            if (choiceButton == null) return;

            string choice = choiceButton.Tag.ToString();
            if (lblMessagePlayer1.Text == "Зробіть хід")
            {
                temporaryPlayer1Choice = choice;
                lblMessagePlayer1.Text = "Очікуємо хід Гравця 2...";
                DisableChoiceButtons();
                btnPlayer2Choice.Enabled = true;
            }
            else if (lblMessagePlayer2.Text == "Зробіть хід")
            {
                temporaryPlayer2Choice = choice;
                lblMessagePlayer2.Text = "Очікуємо хід Гравця 1...";
                DisableChoiceButtons();

                if (!string.IsNullOrEmpty(temporaryPlayer1Choice) && !string.IsNullOrEmpty(temporaryPlayer2Choice))
                {
                    player1Choice = temporaryPlayer1Choice;
                    player2Choice = temporaryPlayer2Choice;

                    player1ChoiceBox.Image = Image.FromFile($"Resources/{player1Choice}.jpg");
                    player2ChoiceBox.Image = Image.FromFile($"Resources/{player2Choice}.jpg");

                    CheckWinner();
                }
            }
        }

        private string TranslateChoice(string choice)
        {
            return choice switch
            {
                "rock" => "камінь",
                "paper" => "папір",
                "scissors" => "ножиці",
                _ => choice
            };
        }

        private void CheckWinner()
        {
            string result;
            string winnerName = "Нічия";

            if (player1Choice == player2Choice)
            {
                result = "Нічия";
            }
            else if ((player1Choice == "rock" && player2Choice == "scissors") ||
                     (player1Choice == "paper" && player2Choice == "rock") ||
                     (player1Choice == "scissors" && player2Choice == "paper"))
            {
                result = "Перемога";
                winnerName = GetUserName(player1Email);

                DataManager.AddGameResult(player1Email, player2Email, result, TranslateChoice(player1Choice ?? string.Empty), TranslateChoice(player2Choice ?? string.Empty));
                DataManager.AddGameResult(player2Email, player1Email, "Поразка", TranslateChoice(player2Choice ?? string.Empty), TranslateChoice(player1Choice ?? string.Empty));
            }
            else
            {
                result = "Перемога";
                winnerName = GetUserName(player2Email);

                DataManager.AddGameResult(player2Email, player1Email, "Поразка", TranslateChoice(player2Choice ?? string.Empty), TranslateChoice(player1Choice ?? string.Empty));
                DataManager.AddGameResult(player1Email, player2Email, result, TranslateChoice(player1Choice ?? string.Empty), TranslateChoice(player2Choice ?? string.Empty));
            }

            resultLabel.Text = $"{result} ({winnerName})";

            var customMessageBox = new CustomMessageBox($"Бажаєте зіграти ще раз?", "Так", "Ні");
            if (customMessageBox.ShowDialog() == DialogResult.Yes)
            {
                ResetGame();
            }
            else
            {
                this.Close();
            }
        }

        private void ResetGame()
        {
            player1Choice = null;
            player2Choice = null;
            temporaryPlayer1Choice = null;
            temporaryPlayer2Choice = null;
            player1ChoiceBox.Image = null;
            player2ChoiceBox.Image = null;
            resultLabel.Text = "";
            EnablePlayerButtons();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnableChoiceButtons()
        {
            btnRock.Enabled = true;
            btnPaper.Enabled = true;
            btnScissors.Enabled = true;
        }

        private void DisableChoiceButtons()
        {
            btnRock.Enabled = false;
            btnPaper.Enabled = false;
            btnScissors.Enabled = false;
        }

        private void EnablePlayerButtons()
        {
            btnPlayer1Choice.Enabled = true;
            btnPlayer2Choice.Enabled = true;
        }

        private void DisablePlayerButtons()
        {
            btnPlayer1Choice.Enabled = false;
            btnPlayer2Choice.Enabled = false;
        }
    }
}