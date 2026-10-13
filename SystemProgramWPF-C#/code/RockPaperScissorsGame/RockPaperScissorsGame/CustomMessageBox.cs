using System;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string questionMessage, string yesText, string noText)
        {
            InitializeComponent();
            lblQuestionMessage.Text = questionMessage; // Запитання
            btnYes.Text = yesText;
            btnNo.Text = noText;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}