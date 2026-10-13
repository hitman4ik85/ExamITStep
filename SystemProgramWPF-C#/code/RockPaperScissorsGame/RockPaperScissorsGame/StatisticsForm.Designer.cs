namespace RockPaperScissorsGame
{
    partial class StatisticsForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lstStatistics = new System.Windows.Forms.ListBox();
            this.btnClearStatistics = new System.Windows.Forms.Button();
            this.btnClearSelectedStat = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstStatistics
            // 
            this.lstStatistics.FormattingEnabled = true;
            this.lstStatistics.HorizontalScrollbar = true;
            this.lstStatistics.ItemHeight = 16;
            this.lstStatistics.Location = new System.Drawing.Point(12, 12);
            this.lstStatistics.Name = "lstStatistics";
            this.lstStatistics.Size = new System.Drawing.Size(560, 324);
            this.lstStatistics.TabIndex = 0;
            // 
            // btnClearStatistics
            // 
            this.btnClearStatistics.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.btnClearStatistics.ForeColor = System.Drawing.Color.White;
            this.btnClearStatistics.Location = new System.Drawing.Point(12, 350);
            this.btnClearStatistics.Name = "btnClearStatistics";
            this.btnClearStatistics.Size = new System.Drawing.Size(150, 40);
            this.btnClearStatistics.TabIndex = 1;
            this.btnClearStatistics.Text = "Очистити всю статистику";
            this.btnClearStatistics.UseVisualStyleBackColor = false;
            this.btnClearStatistics.Click += new System.EventHandler(this.btnClearStatistics_Click);
            // 
            // btnClearSelectedStat
            // 
            this.btnClearSelectedStat.BackColor = System.Drawing.Color.MediumPurple;
            this.btnClearSelectedStat.ForeColor = System.Drawing.Color.White;
            this.btnClearSelectedStat.Location = new System.Drawing.Point(218, 350);
            this.btnClearSelectedStat.Name = "btnClearSelectedStat";
            this.btnClearSelectedStat.Size = new System.Drawing.Size(150, 40);
            this.btnClearSelectedStat.TabIndex = 2;
            this.btnClearSelectedStat.Text = "Очистити обране";
            this.btnClearSelectedStat.UseVisualStyleBackColor = false;
            this.btnClearSelectedStat.Click += new System.EventHandler(this.btnClearSelectedStat_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Crimson;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(422, 350);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Закрити";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 402);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClearSelectedStat);
            this.Controls.Add(this.btnClearStatistics);
            this.Controls.Add(this.lstStatistics);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Статистика";
            this.Load += new System.EventHandler(this.StatisticsForm_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox lstStatistics;
        private System.Windows.Forms.Button btnClearStatistics;
        private System.Windows.Forms.Button btnClearSelectedStat;
        private System.Windows.Forms.Button btnClose;
    }
}