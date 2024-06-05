using MySqlConnector;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Курсач_сайко_1125
{
    public partial class Dn_Registred : Window, INotifyPropertyChanged
    {
        private readonly string _connectionString = "server=localhost;user=root;password=student;database=dayn1";
        private readonly int MaxGroupSize = 25;

        private string _name;
        private string _gpa;
        private string _selectedSpec;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Gpa
        {
            get => _gpa;
            set
            {
                _gpa = value;
                OnPropertyChanged();
            }
        }

        public string SelectedSpec
        {
            get => _selectedSpec;
            set
            {
                _selectedSpec = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Dn_Registred()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Gpa))
            {
                MessageBox.Show("Обязательные поля не заполнены!");
                return;
            }

            if (!decimal.TryParse(Gpa, out decimal gpa))
            {
                MessageBox.Show("Некорректное значение Среднего балла");
                return;
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
                        countCommand.Parameters.AddWithValue("@spec", SelectedSpec);
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
                        command.Parameters.AddWithValue("@name", Name);
                        command.Parameters.AddWithValue("@gpa", gpa);
                        command.Parameters.AddWithValue("@spec", SelectedSpec);
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