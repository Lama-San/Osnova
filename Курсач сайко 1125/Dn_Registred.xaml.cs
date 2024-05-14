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
    /// <summary>
    /// Логика взаимодействия для Dn_Registred.xaml
    /// </summary>
    public partial class Dn_Registred : Window
    {
        public Dn_Registred()
        {
            InitializeComponent();
        }


            private readonly string connectionString = "server=localhost;user=root;password=student;database=dayn1";
               
       
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtGpa.Text))
            {
                MessageBox.Show("Please enter a name and car number.");
                return;
            }
            string name = txtName.Text;
            string GPA = txtGpa.Text; string spec = cboSpec.SelectedItem.ToString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO zap (name, GPA, spec) VALUES (@name, @GPA, @spec)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@GPA", GPA); command.Parameters.AddWithValue("@spec", spec);
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show($"Регистрация успешна, ждите и варитесь(");
            Close();
        }
    }
}
        

    
