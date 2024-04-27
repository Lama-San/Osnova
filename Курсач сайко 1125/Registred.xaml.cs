using System;
using System.Windows;
using Microsoft.Data.SqlClient;

namespace Курсач_сайко_1125
{
    public partial class Registred : Window
    {
        public Registred()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=(local);Initial Catalog=UserRegistration;Integrated Security=True";
            string username = txtUsername.Text;
            string password = pwbPassword.Password;
            string confirmPassword = pwbConfirmPassword.Password;
            string email = txtEmail.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword) || string.IsNullOrEmpty(email))
            {
                lblMessage.Text = "Все поля обязательны для заполнения.";
                return;
            }

            if (password != confirmPassword)
            {
                lblMessage.Text = "Пароли не совпадают.";
                return;
            }

            using (Microsoft.Data.SqlClient.SqlConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                Microsoft.Data.SqlClient.SqlCommand command = new Microsoft.Data.SqlClient.SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    command.ExecuteNonQuery();
                    lblMessage.Text = "Пользователь успешно зарегистрировался.";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Ошибка при регистрации пользователя: " + ex.Message;
                }
            }
        }
    }
}