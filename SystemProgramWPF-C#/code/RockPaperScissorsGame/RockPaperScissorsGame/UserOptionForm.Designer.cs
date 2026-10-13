namespace RockPaperScissorsGame
{
    partial class UserOptionForm
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
            btnLogin = new Button();
            btnRegister = new Button();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.LightSkyBlue;
            btnLogin.Font = new Font("Segoe UI", 12F);
            btnLogin.ForeColor = Color.DarkBlue;
            btnLogin.Location = new Point(52, 38);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(200, 50);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Увійти";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.LightCoral;
            btnRegister.Font = new Font("Segoe UI", 12F);
            btnRegister.ForeColor = Color.Maroon;
            btnRegister.Location = new Point(52, 148);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(200, 50);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Створити";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += btnRegister_Click;
            // 
            // UserOptionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 237);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UserOptionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Меню користувача";
            ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
    }
}