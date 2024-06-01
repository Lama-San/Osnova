using MySqlConnector;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Курсач_сайко_1125
{
    public partial class Dn_Registred : Window
    {
        private readonly string _connectionString = "server=localhost;user=root;password=student;database=dayn1";
        private readonly int MaxGroupSize = 25;

        public Dn_Registred()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtGpa.Text))
            {
                MessageBox.Show("Обязательные поля не заполнены!");
                return;
            }

            string name = txtName.Text;
            if (!decimal.TryParse(txtGpa.Text, out decimal gpa))
            {
                MessageBox.Show("Некорректное значение Среднего балла");
                return;
            }

            string spec = "";
            if (cboSpec.SelectedItem is ComboBoxItem item)
            {
                spec = item.Content.ToString();
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if the group is full
                    string countQuery = "SELECT COUNT(*) FROM zap WHERE Spec = @spec";
                    using (MySqlCommand countCommand = new MySqlCommand(countQuery, connection))
                    {
                        countCommand.Parameters.AddWithValue("@spec", spec);
                        int count = Convert.ToInt32(countCommand.ExecuteScalar());

                        if (count >= MaxGroupSize)
                        {
                            MessageBox.Show("Извините, группа набрана");
                            return;
                        }
                    }

                    // Insert the new registration
                    string query = "INSERT INTO zap (Name, Gpa, Spec) VALUES (@name, @gpa, @spec)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@gpa", gpa);
                        command.Parameters.AddWithValue("@spec", spec);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Удачи... она тебе пригодится там...");
                Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}



