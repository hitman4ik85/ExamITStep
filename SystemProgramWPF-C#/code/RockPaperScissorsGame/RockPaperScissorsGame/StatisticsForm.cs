using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RockPaperScissorsGame
{
    public partial class StatisticsForm : Form
    {
        private readonly string userEmail;

        public StatisticsForm(string email)
        {
            InitializeComponent();
            userEmail = email ?? throw new ArgumentNullException(nameof(email));
        }

        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                lstStatistics.Items.Clear();
                string statisticsFilePath = Path.Combine("statistics", $"{userEmail}.statistic.txt");

                if (File.Exists(statisticsFilePath))
                {
                    string[] lines = File.ReadAllLines(statisticsFilePath);

                    if (lines.Any())
                    {
                        lstStatistics.Items.AddRange(lines);
                    }
                    else
                    {
                        lstStatistics.Items.Add("Немає записів у статистиці.");
                    }
                }
                else
                {
                    MessageBox.Show("Файл статистики не знайдено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завантаження статистики: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                DataManager.ClearStatistics(userEmail);
                lstStatistics.Items.Clear();
                MessageBox.Show("Статистика успішно очищена!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка очищення статистики: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearSelectedStat_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstStatistics.SelectedItem != null)
                {
                    string selectedStat = lstStatistics.SelectedItem.ToString();
                    string statisticsFilePath = Path.Combine("statistics", $"{userEmail}.statistic.txt");

                    var lines = File.ReadAllLines(statisticsFilePath).ToList();
                    lines.Remove(selectedStat);
                    File.WriteAllLines(statisticsFilePath, lines);

                    MessageBox.Show("Обраний запис видалено!", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStatistics();
                }
                else
                {
                    MessageBox.Show("Оберіть запис для видалення!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка видалення запису: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}