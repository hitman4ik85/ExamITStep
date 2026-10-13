namespace RockPaperScissorsGame
{
    partial class AdminMenuForm
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
            this.btnViewPlayers = new System.Windows.Forms.Button();
            this.btnChangeAdminCredentials = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnViewPlayers
            // 
            this.btnViewPlayers.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnViewPlayers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnViewPlayers.ForeColor = System.Drawing.Color.White;
            this.btnViewPlayers.Location = new System.Drawing.Point(100, 50);
            this.btnViewPlayers.Name = "btnViewPlayers";
            this.btnViewPlayers.Size = new System.Drawing.Size(200, 50);
            this.btnViewPlayers.TabIndex = 0;
            this.btnViewPlayers.Text = "Перегляд гравців";
            this.btnViewPlayers.UseVisualStyleBackColor = false;
            this.btnViewPlayers.Click += new System.EventHandler(this.btnViewPlayers_Click);
            // 
            // btnChangeAdminCredentials
            // 
            this.btnChangeAdminCredentials.BackColor = System.Drawing.Color.MediumPurple;
            this.btnChangeAdminCredentials.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChangeAdminCredentials.ForeColor = System.Drawing.Color.White;
            this.btnChangeAdminCredentials.Location = new System.Drawing.Point(100, 150);
            this.btnChangeAdminCredentials.Name = "btnChangeAdminCredentials";
            this.btnChangeAdminCredentials.Size = new System.Drawing.Size(200, 50);
            this.btnChangeAdminCredentials.TabIndex = 1;
            this.btnChangeAdminCredentials.Text = "Змінити логін/пароль";
            this.btnChangeAdminCredentials.UseVisualStyleBackColor = false;
            this.btnChangeAdminCredentials.Click += new System.EventHandler(this.btnChangeAdminCredentials_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Crimson;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(100, 250);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 50);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Вийти";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // AdminMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnChangeAdminCredentials);
            this.Controls.Add(this.btnViewPlayers);
            this.Name = "AdminMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Меню адміністратора";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnViewPlayers;
        private System.Windows.Forms.Button btnChangeAdminCredentials;
        private System.Windows.Forms.Button btnExit;
    }
}