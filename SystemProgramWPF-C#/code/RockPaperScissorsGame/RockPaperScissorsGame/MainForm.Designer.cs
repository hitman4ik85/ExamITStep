namespace RockPaperScissorsGame
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAdminMenu;
        private System.Windows.Forms.Button btnUserMenu;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem infoMenuItem;
        private System.Windows.Forms.Button btnMute;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnAdminMenu = new System.Windows.Forms.Button();
            this.btnUserMenu = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.infoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMute = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // 
            // btnAdminMenu
            // 
            this.btnAdminMenu.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnAdminMenu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdminMenu.ForeColor = System.Drawing.Color.White;
            this.btnAdminMenu.Location = new System.Drawing.Point(200, 100);
            this.btnAdminMenu.Name = "btnAdminMenu";
            this.btnAdminMenu.Size = new System.Drawing.Size(200, 50);
            this.btnAdminMenu.TabIndex = 0;
            this.btnAdminMenu.Text = "Меню адміністратора";
            this.btnAdminMenu.UseVisualStyleBackColor = false;
            this.btnAdminMenu.Click += new System.EventHandler(this.btnAdminMenu_Click);

            // 
            // btnUserMenu
            // 
            this.btnUserMenu.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnUserMenu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserMenu.ForeColor = System.Drawing.Color.White;
            this.btnUserMenu.Location = new System.Drawing.Point(200, 180);
            this.btnUserMenu.Name = "btnUserMenu";
            this.btnUserMenu.Size = new System.Drawing.Size(200, 50);
            this.btnUserMenu.TabIndex = 1;
            this.btnUserMenu.Text = "Меню користувача";
            this.btnUserMenu.UseVisualStyleBackColor = false;
            this.btnUserMenu.Click += new System.EventHandler(this.btnUserMenu_Click);

            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Crimson;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(200, 260);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 50);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Вийти";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);

            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(600, 24);
            this.mainMenuStrip.TabIndex = 3;
            this.mainMenuStrip.Text = "menuStrip1";
            this.mainMenuStrip.Dock = System.Windows.Forms.DockStyle.Top;

            // 
            // infoMenuItem
            // 
            this.infoMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right; // Вирівнювання вправо
            this.infoMenuItem.Name = "infoMenuItem";
            this.infoMenuItem.Size = new System.Drawing.Size(43, 20);
            this.infoMenuItem.Text = "Info";
            this.infoMenuItem.Click += new System.EventHandler(this.infoMenuItem_Click);

            // 
            // btnMute
            // 
            this.btnMute.Location = new System.Drawing.Point(540, 30); // Розташування кнопки під Info
            this.btnMute.Size = new System.Drawing.Size(40, 40);
            this.btnMute.Name = "btnMute";
            this.btnMute.BackgroundImage = Image.FromFile("Resources/mute.jpg");
            this.btnMute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 400);
            this.Controls.Add(this.btnMute);
            this.Controls.Add(this.mainMenuStrip);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnUserMenu);
            this.Controls.Add(this.btnAdminMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Головне меню";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}