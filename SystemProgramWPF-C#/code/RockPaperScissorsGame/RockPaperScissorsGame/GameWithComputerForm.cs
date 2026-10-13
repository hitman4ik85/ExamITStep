using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class GameWithComputerForm : Form
    {
        private readonly string playerEmail;
        private string? playerChoice;
        private string? computerChoice;

        public GameWithComputerForm(string email)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Центруємо форму
            playerEmail = email ?? throw new ArgumentNullException(nameof(email));
            SetBackgroundImage();
            LoadButtonImages();
            ResetGame();
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

        private async void btnRock_Click(object sender, EventArgs e)
        {
            await HandlePlayerChoice("rock");
        }

        private async void btnPaper_Click(object sender, EventArgs e)
        {
            await HandlePlayerChoice("paper");
        }

        private async void btnScissors_Click(object sender, EventArgs e)
        {
            await HandlePlayerChoice("scissors");
        }

        private async Task HandlePlayerChoice(string choice)
        {
            try
            {
                playerChoice = choice;
                playerChoiceBox.Image = Image.FromFile($"Resources/{choice}.jpg");

                await Task.Delay(500); // Затримка перед вибором комп'ютера
                computerChoice = GetComputerChoice();
                computerChoiceBox.Image = Image.FromFile($"Resources/{computerChoice}.jpg");

                await Task.Delay(500); // Затримка перед відображенням результату
                ShowResult();
                ResetGame();
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"Не вдалося знайти файл зображення: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка під час обробки вибору: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetComputerChoice()
        {
            var choices = new[] { "rock", "paper", "scissors" };
            var random = new Random();
            return choices[random.Next(choices.Length)];
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

        private void ShowResult()
        {
            string playerName = DataManager.GetUserName(playerEmail);
            string result;

            if (playerChoice == computerChoice)
            {
                result = "Нічия!";
            }
            else if ((playerChoice == "rock" && computerChoice == "scissors") ||
                     (playerChoice == "paper" && computerChoice == "rock") ||
                     (playerChoice == "scissors" && computerChoice == "paper"))
            {
                result = $"{playerName} виграв!";
            }
            else
            {
                result = "Комп'ютер виграв!";
            }

            resultLabel.Text = result;

            DataManager.AddGameResult(playerEmail, "Комп'ютер", result, TranslateChoice(playerChoice ?? string.Empty), TranslateChoice(computerChoice ?? string.Empty));
        }

        private void ResetGame()
        {
            playerChoice = null;
            computerChoice = null;
            playerChoiceBox.Image = null; // Очищаємо зображення вибору
            computerChoiceBox.Image = null; // Очищаємо зображення вибору
            playerPictureBox.Image = Image.FromFile("Resources/player.jpg"); // Встановлюємо стартову картинку гравця
            computerPictureBox.Image = Image.FromFile("Resources/computer.jpg"); // Встановлюємо стартову картинку комп'ютера
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private Image ResizeImage(string filePath, int width, int height)
        {
            using (var originalImage = Image.FromFile(filePath))
            {
                var resizedImage = new Bitmap(originalImage, new Size(width, height));
                return resizedImage;
            }
        }
    }
}