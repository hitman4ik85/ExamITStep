using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class PlayerManagementForm : Form
    {
        public PlayerManagementForm()
        {
            InitializeComponent();
            LoadPlayers();
            this.KeyPreview = true;
            this.KeyDown += PlayerManagementForm_KeyDown;
        }

        private void LoadPlayers()
        {
            lstPlayers.Items.Clear();
            if (File.Exists("accounts.txt"))
            {
                var accounts = File.ReadAllLines("accounts.txt");
                foreach (var account in accounts)
                {
                    var parts = account.Split(',');
                    if (parts.Length > 1)
                    {
                        // Ім'я гравця та email у дужках
                        lstPlayers.Items.Add($"{parts[0]} ({parts[1]})");
                    }
                }
            }
        }

        private void btnDeletePlayer_Click(object sender, EventArgs e)
        {
            if (lstPlayers.SelectedItem != null)
            {
                string selectedPlayer = lstPlayers.SelectedItem.ToString();

                // Отримуємо email гравця
                string email = selectedPlayer.Substring(selectedPlayer.IndexOf('(') + 1).TrimEnd(')');

                try
                {
                    DataManager.RemoveUser(email); // Видалення користувача і його статистики
                    MessageBox.Show("Гравця та його статистику успішно видалено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadPlayers(); // Оновлення списку гравців
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при видаленні гравця: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть гравця для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlayerManagementForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                btnClose_Click(sender, e);
            }
        }
    }
}