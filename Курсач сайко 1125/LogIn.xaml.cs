using CollegeAdmissionAutomation;
using Kinoteka2._0;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Курсач_сайко_1125
{
    public partial class LogIn : Window, INotifyPropertyChanged
    {
        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get; set; }
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public LogIn()
        {
            InitializeComponent();
            DataContext = this;
        }
        public string ErrorMesage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                Signal();
            }
        }

        private void CheckAuth(string login, string pass)
        {
            //Проверка на правильный ввод данных

            //Проверка на правильный ввод данных
            ErrorMesage = null;
            var user = DB.Instance.Logins.

                FirstOrDefault(s => s.FirstName == login && s.Password == pass);
            if (user != null)
            {
                new MainWindow(user).Show();
                Close();
            }
            else if (login == null)
            {
                ErrorMesage = "Неправильно указан логин или пароль!";

            }


        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            CheckAuth(Login, passwordBox.Password);
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Register window.
            Registred registred = new Registred();

            // Show the Register window.
            registred.Show();


        }   
    }
}