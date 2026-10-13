namespace RockPaperScissorsGame
{
    partial class GameWithComputerForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox playerPictureBox;
        private PictureBox computerPictureBox;
        private PictureBox playerChoiceBox;
        private PictureBox computerChoiceBox;
        private Label resultLabel;
        private Button btnRock;
        private Button btnPaper;
        private Button btnScissors;
        private Button btnBack;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            playerPictureBox = new PictureBox();
            computerPictureBox = new PictureBox();
            playerChoiceBox = new PictureBox();
            computerChoiceBox = new PictureBox();
            resultLabel = new Label();
            btnRock = new Button();
            btnPaper = new Button();
            btnScissors = new Button();
            btnBack = new Button();
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)computerPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerChoiceBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)computerChoiceBox).BeginInit();
            SuspendLayout();
            // 
            // playerPictureBox
            // 
            playerPictureBox.Location = new Point(50, 50);
            playerPictureBox.Name = "playerPictureBox";
            playerPictureBox.Size = new Size(150, 150);
            playerPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            playerPictureBox.TabIndex = 0;
            playerPictureBox.TabStop = false;
            // 
            // computerPictureBox
            // 
            computerPictureBox.Location = new Point(500, 50);
            computerPictureBox.Name = "computerPictureBox";
            computerPictureBox.Size = new Size(150, 150);
            computerPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            computerPictureBox.TabIndex = 1;
            computerPictureBox.TabStop = false;
            // 
            // playerChoiceBox
            // 
            playerChoiceBox.BorderStyle = BorderStyle.FixedSingle;
            playerChoiceBox.Location = new Point(220, 50);
            playerChoiceBox.Name = "playerChoiceBox";
            playerChoiceBox.Size = new Size(115, 114);
            playerChoiceBox.SizeMode = PictureBoxSizeMode.StretchImage;
            playerChoiceBox.TabIndex = 2;
            playerChoiceBox.TabStop = false;
            // 
            // computerChoiceBox
            // 
            computerChoiceBox.BorderStyle = BorderStyle.FixedSingle;
            computerChoiceBox.Location = new Point(370, 50);
            computerChoiceBox.Name = "computerChoiceBox";
            computerChoiceBox.Size = new Size(110, 114);
            computerChoiceBox.SizeMode = PictureBoxSizeMode.StretchImage;
            computerChoiceBox.TabIndex = 3;
            computerChoiceBox.TabStop = false;
            // 
            // resultLabel
            // 
            resultLabel.BackColor = Color.FromArgb(200, 255, 255, 255);
            resultLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            resultLabel.ForeColor = Color.Red;
            resultLabel.Location = new Point(201, 217);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(300, 36);
            resultLabel.TabIndex = 4;
            resultLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRock
            // 
            btnRock.Location = new Point(150, 300);
            btnRock.Name = "btnRock";
            btnRock.Size = new Size(100, 100);
            btnRock.TabIndex = 5;
            btnRock.Click += btnRock_Click;
            // 
            // btnPaper
            // 
            btnPaper.Location = new Point(300, 300);
            btnPaper.Name = "btnPaper";
            btnPaper.Size = new Size(100, 100);
            btnPaper.TabIndex = 6;
            btnPaper.Click += btnPaper_Click;
            // 
            // btnScissors
            // 
            btnScissors.Location = new Point(450, 300);
            btnScissors.Name = "btnScissors";
            btnScissors.Size = new Size(100, 100);
            btnScissors.TabIndex = 7;
            btnScissors.Click += btnScissors_Click;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Crimson;
            btnBack.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(276, 443);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(150, 50);
            btnBack.TabIndex = 8;
            btnBack.Text = "Назад";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // GameWithComputerForm
            // 
            ClientSize = new Size(700, 543);
            Controls.Add(playerPictureBox);
            Controls.Add(computerPictureBox);
            Controls.Add(playerChoiceBox);
            Controls.Add(computerChoiceBox);
            Controls.Add(resultLabel);
            Controls.Add(btnRock);
            Controls.Add(btnPaper);
            Controls.Add(btnScissors);
            Controls.Add(btnBack);
            Name = "GameWithComputerForm";
            Text = "Гра проти Комп'ютера";
            ((System.ComponentModel.ISupportInitialize)playerPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)computerPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerChoiceBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)computerChoiceBox).EndInit();
            ResumeLayout(false);
        }
    }
}