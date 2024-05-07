using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CollegeAdmissionAutomation;
using BD;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
namespace Курсач_сайко_1125
{
    public partial class Registred : Window
    {
        private Login login;

        public string Login { get; set; }
        public string Email { get; set; }
        public PasswordBox Password { get; set; }
        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


        public Registred()
        {
            //Перевод вводимого текста в данные
            InitializeComponent();
            DataContext = this;
            Password = pwbPassword;

        }
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                Signal();
            }
        }
        private int GetRoleIdFromDatabase()
        {
            
            using (var context = new Dayn1Context())
            {              
                int roleId = context.Roles.FirstOrDefault()?.Id ?? default;

                return roleId;
            }
        }
        private void Check_Register(object sender, RoutedEventArgs e)
        {
            if (txtUsername == null || pwbPassword == null || txtEmail == null)
            {
                ErrorMessage = "Обязательные поля не заполнены";
                return;
            }
            else
            {
                int roleId = GetRoleIdFromDatabase();

                var login = new Login
                {
                    FirstName = txtUsername.Text,
                    Password = Password.Password,
                    Email = Email,
                    RoleId = roleId
                };

                login.Id = DB.GetInstance().Logins.Any() ? DB.GetInstance().Logins.Max(l => l.Id) + 1 : 1;
                DB.GetInstance().Logins.Add(login);
                DB.GetInstance().SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!");
                this.Close();
                new MainWindow(login).Show();
              
            }
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Check_Register(sender, e);
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Определите формат адреса электронной почты с помощью регулярного выражения
            string emailFormat = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Создать объект регулярного выражения
            Regex emailRegex = new Regex(emailFormat);
          
        }

      
    }
}