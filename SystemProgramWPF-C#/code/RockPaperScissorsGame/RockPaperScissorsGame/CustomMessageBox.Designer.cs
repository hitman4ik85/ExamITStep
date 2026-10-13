namespace RockPaperScissorsGame
{
    partial class CustomMessageBox
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
            lblQuestionMessage = new Label();
            btnYes = new Button();
            btnNo = new Button();
            SuspendLayout();
            // 
            // lblQuestionMessage
            // 
            lblQuestionMessage.Font = new Font("Segoe UI", 10F);
            lblQuestionMessage.Location = new Point(31, 24);
            lblQuestionMessage.Name = "lblQuestionMessage";
            lblQuestionMessage.Size = new Size(240, 23);
            lblQuestionMessage.TabIndex = 1;
            lblQuestionMessage.Text = "Бажаєте зіграти ще раз?";
            lblQuestionMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnYes
            // 
            btnYes.BackColor = Color.LightSkyBlue;
            btnYes.Font = new Font("Segoe UI", 12F);
            btnYes.ForeColor = Color.DarkBlue;
            btnYes.Location = new Point(41, 64);
            btnYes.Name = "btnYes";
            btnYes.Size = new Size(100, 40);
            btnYes.TabIndex = 2;
            btnYes.Text = "Так";
            btnYes.UseVisualStyleBackColor = false;
            btnYes.Click += btnYes_Click;
            // 
            // btnNo
            // 
            btnNo.BackColor = Color.LightCoral;
            btnNo.Font = new Font("Segoe UI", 12F);
            btnNo.ForeColor = Color.Maroon;
            btnNo.Location = new Point(161, 64);
            btnNo.Name = "btnNo";
            btnNo.Size = new Size(100, 40);
            btnNo.TabIndex = 3;
            btnNo.Text = "Ні";
            btnNo.UseVisualStyleBackColor = false;
            btnNo.Click += btnNo_Click;
            // 
            // CustomMessageBox
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 129);
            Controls.Add(btnNo);
            Controls.Add(btnYes);
            Controls.Add(lblQuestionMessage);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CustomMessageBox";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Гра завершена";
            ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblQuestionMessage;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
    }
}