using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
namespace Курсач_сайко_1125
{
    public partial class Dn_Registred : Window
    {
        private readonly string _connectionString = "server=localhost;user=root;password=student;database=dayn1";

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
            decimal gpa;
            if (!decimal.TryParse(txtGpa.Text, out gpa))
            {
                MessageBox.Show("Некорректное значение GPA");
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
                    string query = "INSERT INTO zap (Name, Gpa, Spec) VALUES (@name, @gpa, @spec)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@gpa", gpa);
                        command.Parameters.AddWithValue("@spec", spec);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show($"Регистрация успешна, ждите и варитесь(");
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



