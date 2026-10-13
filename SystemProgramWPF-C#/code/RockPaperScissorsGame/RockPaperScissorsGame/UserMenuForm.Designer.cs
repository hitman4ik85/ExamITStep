namespace RockPaperScissorsGame
{
    partial class UserMenuForm
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
            this.btnPlayWithComputer = new System.Windows.Forms.Button();
            this.btnPlayWithPlayer = new System.Windows.Forms.Button();
            this.btnViewStatistics = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnChangeUserData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPlayWithComputer
            // 
            this.btnPlayWithComputer.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnPlayWithComputer.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnPlayWithComputer.ForeColor = System.Drawing.Color.White;
            this.btnPlayWithComputer.Location = new System.Drawing.Point(100, 50);
            this.btnPlayWithComputer.Name = "btnPlayWithComputer";
            this.btnPlayWithComputer.Size = new System.Drawing.Size(200, 50);
            this.btnPlayWithComputer.TabIndex = 0;
            this.btnPlayWithComputer.Text = "Грати з комп'ютером";
            this.btnPlayWithComputer.UseVisualStyleBackColor = false;
            this.btnPlayWithComputer.Click += new System.EventHandler(this.btnPlayWithComputer_Click);
            // 
            // btnPlayWithPlayer
            // 
            this.btnPlayWithPlayer.BackColor = System.Drawing.Color.MediumPurple;
            this.btnPlayWithPlayer.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnPlayWithPlayer.ForeColor = System.Drawing.Color.White;
            this.btnPlayWithPlayer.Location = new System.Drawing.Point(100, 120);
            this.btnPlayWithPlayer.Name = "btnPlayWithPlayer";
            this.btnPlayWithPlayer.Size = new System.Drawing.Size(200, 50);
            this.btnPlayWithPlayer.TabIndex = 1;
            this.btnPlayWithPlayer.Text = "Грати з гравцем";
            this.btnPlayWithPlayer.UseVisualStyleBackColor = false;
            this.btnPlayWithPlayer.Click += new System.EventHandler(this.btnPlayWithPlayer_Click);
            // 
            // btnViewStatistics
            // 
            this.btnViewStatistics.BackColor = System.Drawing.Color.SteelBlue;
            this.btnViewStatistics.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnViewStatistics.ForeColor = System.Drawing.Color.White;
            this.btnViewStatistics.Location = new System.Drawing.Point(100, 190);
            this.btnViewStatistics.Name = "btnViewStatistics";
            this.btnViewStatistics.Size = new System.Drawing.Size(200, 50);
            this.btnViewStatistics.TabIndex = 2;
            this.btnViewStatistics.Text = "Статистика";
            this.btnViewStatistics.UseVisualStyleBackColor = false;
            this.btnViewStatistics.Click += new System.EventHandler(this.btnViewStatistics_Click);
            // 
            // btnChangeUserData
            // 
            this.btnChangeUserData.BackColor = System.Drawing.Color.Teal;
            this.btnChangeUserData.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnChangeUserData.ForeColor = System.Drawing.Color.White;
            this.btnChangeUserData.Location = new System.Drawing.Point(100, 260);
            this.btnChangeUserData.Name = "btnChangeUserData";
            this.btnChangeUserData.Size = new System.Drawing.Size(200, 50);
            this.btnChangeUserData.TabIndex = 4;
            this.btnChangeUserData.Text = "Змінити дані";
            this.btnChangeUserData.UseVisualStyleBackColor = false;
            this.btnChangeUserData.Click += new System.EventHandler(this.btnChangeUserData_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.Crimson;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(100, 330);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(200, 50);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Вийти";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // UserMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnChangeUserData);
            this.Controls.Add(this.btnViewStatistics);
            this.Controls.Add(this.btnPlayWithPlayer);
            this.Controls.Add(this.btnPlayWithComputer);
            this.Name = "UserMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Меню гравця";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnPlayWithComputer;
        private System.Windows.Forms.Button btnPlayWithPlayer;
        private System.Windows.Forms.Button btnViewStatistics;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnChangeUserData;
    }
}