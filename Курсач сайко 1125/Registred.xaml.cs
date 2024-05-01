using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using CollegeAdmissionAutomation;
using Kinoteka2._0;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
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

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string password = Password.Password;
           

            MessageBox.Show("Регистрация прошла успешно!");
            login = new Login
            {
                FirstName = Login,
                Password = password,

                Email = login.Email,
               
            };
            DB.GetInstance().Logins.Add(login);
            DB.GetInstance().SaveChanges();
            this.Close();
            new MainWindow(login).Show();
        }
    }
}