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
        private string PassportNumber;
        public Dn_Registred(string passportNumber) // Измените конструктор
        {
            InitializeComponent();
            PassportNumber = passportNumber; // Сохраните номер паспорта
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
            if (cmbSpeciality.SelectedItem is ComboBoxItem item)
            {
                spec = item.Content.ToString();
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();

                    // Проверка, не полна ли группа
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

                    // Вставка новой регистрации в таблицу zap
                    string query = "INSERT INTO zap (Name, Gpa, Spec, PassportNumber) VALUES (@name, @gpa, @spec, @passportNumber)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@gpa", gpa);
                        command.Parameters.AddWithValue("@spec", spec);
                        command.Parameters.AddWithValue("@passportNumber", PassportNumber); // Передача номера паспорта
                        command.ExecuteNonQuery();
                    }

                    // Обновление статуса в таблице loginst
                    string updateStatusQuery = "UPDATE loginst SET Status = 'На рассмотрении' WHERE PassportNumber = @passportNumber";
                    using (MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, connection))
                    {
                        updateStatusCommand.Parameters.AddWithValue("@passportNumber", PassportNumber);
                        updateStatusCommand.ExecuteNonQuery();
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