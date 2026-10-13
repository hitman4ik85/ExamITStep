namespace RockPaperScissorsGame
{
    partial class GameWithPlayerForm
    {
        private System.ComponentModel.IContainer components = null;

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
            lblPlayer1 = new Label();
            lblPlayer2 = new Label();
            lblMessagePlayer1 = new Label();
            lblMessagePlayer2 = new Label();
            resultLabel = new Label();
            btnRock = new Button();
            btnPaper = new Button();
            btnScissors = new Button();
            btnPlayer1Choice = new Button();
            btnPlayer2Choice = new Button();
            btnPlayer2Login = new Button();
            btnBack = new Button();
            player1ChoiceBox = new PictureBox();
            player2ChoiceBox = new PictureBox();
            player1PictureBox = new PictureBox();
            player2PictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)player1ChoiceBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)player2ChoiceBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)player1PictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)player2PictureBox).BeginInit();
            SuspendLayout();
            // 
            // lblPlayer1
            // 
            lblPlayer1.Font = new Font("Segoe UI", 12F);
            lblPlayer1.Location = new Point(50, 20);
            lblPlayer1.Name = "lblPlayer1";
            lblPlayer1.Size = new Size(150, 30);
            lblPlayer1.TabIndex = 0;
            lblPlayer1.Text = "Player 1";
            lblPlayer1.BackColor = Color.FromArgb(200, 255, 255, 255); // Напівпрозорий фон
            // 
            // lblPlayer2
            // 
            lblPlayer2.Font = new Font("Segoe UI", 12F);
            lblPlayer2.Location = new Point(500, 20);
            lblPlayer2.Name = "lblPlayer2";
            lblPlayer2.Size = new Size(150, 30);
            lblPlayer2.TabIndex = 1;
            lblPlayer2.Text = "Unknown";
            lblPlayer2.BackColor = Color.FromArgb(200, 255, 255, 255); // Напівпрозорий фон
            // 
            // lblMessagePlayer1
            // 
            lblMessagePlayer1.Font = new Font("Segoe UI", 10F);
            lblMessagePlayer1.ForeColor = Color.DarkGreen;
            lblMessagePlayer1.Location = new Point(50, 300);
            lblMessagePlayer1.Name = "lblMessagePlayer1";
            lblMessagePlayer1.Size = new Size(150, 30);
            lblMessagePlayer1.TabIndex = 2;
            lblMessagePlayer1.BackColor = Color.FromArgb(200, 255, 255, 255); // Напівпрозорий фон
            // 
            // lblMessagePlayer2
            // 
            lblMessagePlayer2.Font = new Font("Segoe UI", 10F);
            lblMessagePlayer2.ForeColor = Color.DarkGreen;
            lblMessagePlayer2.Location = new Point(500, 300);
            lblMessagePlayer2.Name = "lblMessagePlayer2";
            lblMessagePlayer2.Size = new Size(150, 30);
            lblMessagePlayer2.TabIndex = 3;
            lblMessagePlayer2.BackColor = Color.FromArgb(200, 255, 255, 255); // Напівпрозорий фон
            // 
            // resultLabel
            // 
            resultLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            resultLabel.ForeColor = Color.Red;
            resultLabel.Location = new Point(220, 50);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(260, 32);
            resultLabel.TabIndex = 4;
            resultLabel.TextAlign = ContentAlignment.MiddleCenter;
            resultLabel.BackColor = Color.FromArgb(200, 255, 255, 255); // Напівпрозорий фон
            // 
            // btnRock
            // 
            btnRock.Location = new Point(184, 358);
            btnRock.Name = "btnRock";
            btnRock.Size = new Size(80, 80);
            btnRock.TabIndex = 5;
            btnRock.Tag = "rock";
            btnRock.Click += ChoiceButton_Click;
            // 
            // btnPaper
            // 
            btnPaper.Location = new Point(304, 358);
            btnPaper.Name = "btnPaper";
            btnPaper.Size = new Size(80, 80);
            btnPaper.TabIndex = 6;
            btnPaper.Tag = "paper";
            btnPaper.Click += ChoiceButton_Click;
            // 
            // btnScissors
            // 
            btnScissors.Location = new Point(424, 358);
            btnScissors.Name = "btnScissors";
            btnScissors.Size = new Size(80, 80);
            btnScissors.TabIndex = 7;
            btnScissors.Tag = "scissors";
            btnScissors.Click += ChoiceButton_Click;
            // 
            // btnPlayer1Choice
            // 
            btnPlayer1Choice.BackColor = Color.LightBlue;
            btnPlayer1Choice.Font = new Font("Segoe UI", 12F);
            btnPlayer1Choice.Location = new Point(50, 250);
            btnPlayer1Choice.Name = "btnPlayer1Choice";
            btnPlayer1Choice.Size = new Size(150, 40);
            btnPlayer1Choice.TabIndex = 8;
            btnPlayer1Choice.Text = "Хід Гравця 1";
            btnPlayer1Choice.UseVisualStyleBackColor = false;
            btnPlayer1Choice.Click += btnPlayer1Choice_Click;
            // 
            // btnPlayer2Choice
            // 
            btnPlayer2Choice.BackColor = Color.LightCoral;
            btnPlayer2Choice.Font = new Font("Segoe UI", 12F);
            btnPlayer2Choice.Location = new Point(500, 250);
            btnPlayer2Choice.Name = "btnPlayer2Choice";
            btnPlayer2Choice.Size = new Size(150, 40);
            btnPlayer2Choice.TabIndex = 9;
            btnPlayer2Choice.Text = "Хід Гравця 2";
            btnPlayer2Choice.UseVisualStyleBackColor = false;
            btnPlayer2Choice.Click += btnPlayer2Choice_Click;
            // 
            // btnPlayer2Login
            // 
            btnPlayer2Login.BackColor = Color.MediumSlateBlue;
            btnPlayer2Login.Font = new Font("Segoe UI", 12F);
            btnPlayer2Login.ForeColor = Color.White;
            btnPlayer2Login.Location = new Point(500, 200);
            btnPlayer2Login.Name = "btnPlayer2Login";
            btnPlayer2Login.Size = new Size(150, 40);
            btnPlayer2Login.TabIndex = 10;
            btnPlayer2Login.Text = "Увійти до Гри";
            btnPlayer2Login.UseVisualStyleBackColor = false;
            btnPlayer2Login.Click += btnPlayer2Login_Click;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Crimson;
            btnBack.Font = new Font("Segoe UI", 12F);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(273, 475);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(150, 50);
            btnBack.TabIndex = 11;
            btnBack.Text = "Назад";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // player1ChoiceBox
            // 
            player1ChoiceBox.BorderStyle = BorderStyle.FixedSingle;
            player1ChoiceBox.Location = new Point(220, 98);
            player1ChoiceBox.Name = "player1ChoiceBox";
            player1ChoiceBox.Size = new Size(108, 102);
            player1ChoiceBox.SizeMode = PictureBoxSizeMode.StretchImage;
            player1ChoiceBox.TabIndex = 12;
            player1ChoiceBox.TabStop = false;
            // 
            // player2ChoiceBox
            // 
            player2ChoiceBox.BorderStyle = BorderStyle.FixedSingle;
            player2ChoiceBox.Location = new Point(369, 98);
            player2ChoiceBox.Name = "player2ChoiceBox";
            player2ChoiceBox.Size = new Size(111, 102);
            player2ChoiceBox.SizeMode = PictureBoxSizeMode.StretchImage;
            player2ChoiceBox.TabIndex = 13;
            player2ChoiceBox.TabStop = false;
            // 
            // player1PictureBox
            // 
            player1PictureBox.Location = new Point(50, 50);
            player1PictureBox.Name = "player1PictureBox";
            player1PictureBox.Size = new Size(150, 150);
            player1PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            player1PictureBox.TabIndex = 14;
            player1PictureBox.TabStop = false;
            // 
            // player2PictureBox
            // 
            player2PictureBox.Location = new Point(500, 50);
            player2PictureBox.Name = "player2PictureBox";
            player2PictureBox.Size = new Size(150, 150);
            player2PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            player2PictureBox.TabIndex = 15;
            player2PictureBox.TabStop = false;
            // 
            // GameWithPlayerForm
            // 
            ClientSize = new Size(700, 565);
            Controls.Add(lblPlayer1);
            Controls.Add(lblPlayer2);
            Controls.Add(lblMessagePlayer1);
            Controls.Add(lblMessagePlayer2);
            Controls.Add(resultLabel);
            Controls.Add(btnRock);
            Controls.Add(btnPaper);
            Controls.Add(btnScissors);
            Controls.Add(btnPlayer1Choice);
            Controls.Add(btnPlayer2Choice);
            Controls.Add(btnPlayer2Login);
            Controls.Add(btnBack);
            Controls.Add(player1ChoiceBox);
            Controls.Add(player2ChoiceBox);
            Controls.Add(player1PictureBox);
            Controls.Add(player2PictureBox);
            Name = "GameWithPlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Гра з Гравцем";
            ((System.ComponentModel.ISupportInitialize)player1ChoiceBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)player2ChoiceBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)player1PictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)player2PictureBox).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblPlayer1;
        private System.Windows.Forms.Label lblPlayer2;
        private System.Windows.Forms.Label lblMessagePlayer1;
        private System.Windows.Forms.Label lblMessagePlayer2;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Button btnRock;
        private System.Windows.Forms.Button btnPaper;
        private System.Windows.Forms.Button btnScissors;
        private System.Windows.Forms.Button btnPlayer1Choice;
        private System.Windows.Forms.Button btnPlayer2Choice;
        private System.Windows.Forms.Button btnPlayer2Login;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.PictureBox player1ChoiceBox;
        private System.Windows.Forms.PictureBox player2ChoiceBox;
        private System.Windows.Forms.PictureBox player1PictureBox;
        private System.Windows.Forms.PictureBox player2PictureBox;
    }
}