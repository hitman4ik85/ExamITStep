namespace RockPaperScissorsGame
{
    partial class PlayerManagementForm
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
            this.lstPlayers = new System.Windows.Forms.ListBox();
            this.btnDeletePlayer = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstPlayers
            // 
            this.lstPlayers.FormattingEnabled = true;
            this.lstPlayers.ItemHeight = 20;
            this.lstPlayers.Location = new System.Drawing.Point(50, 30);
            this.lstPlayers.Name = "lstPlayers";
            this.lstPlayers.Size = new System.Drawing.Size(300, 200);
            this.lstPlayers.TabIndex = 0;
            // 
            // btnDeletePlayer
            // 
            this.btnDeletePlayer.BackColor = System.Drawing.Color.Crimson;
            this.btnDeletePlayer.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnDeletePlayer.ForeColor = System.Drawing.Color.White;
            this.btnDeletePlayer.Location = new System.Drawing.Point(50, 250);
            this.btnDeletePlayer.Name = "btnDeletePlayer";
            this.btnDeletePlayer.Size = new System.Drawing.Size(150, 50);
            this.btnDeletePlayer.TabIndex = 1;
            this.btnDeletePlayer.Text = "Видалити";
            this.btnDeletePlayer.UseVisualStyleBackColor = false;
            this.btnDeletePlayer.Click += new System.EventHandler(this.btnDeletePlayer_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(220, 250);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 50);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PlayerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDeletePlayer);
            this.Controls.Add(this.lstPlayers);
            this.Name = "PlayerManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Управління гравцями";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox lstPlayers;
        private System.Windows.Forms.Button btnDeletePlayer;
        private System.Windows.Forms.Button btnClose;
    }
}