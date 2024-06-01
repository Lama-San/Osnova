using BD;
using CollegeAdmissionAutomation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Курсач_сайко_1125
{
    /// <summary>
    /// Логика взаимодействия для To_Lk.xaml
    /// </summary>
    public partial class To_Lk : Window, INotifyPropertyChanged
    {
        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get; set; }
        public string PassportNumber { get; set; }
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public To_Lk()
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
            // Проверка на правильный ввод данных
            var user = DB.Instance.Logins.FirstOrDefault(s => s.FirstName == login && s.PassportNumber == pass);
            if (user != null)
            {
                // If the login is successful, open the new window
                Lk lk = new Lk();
                lk.Show();
                Close();
            }
            else if (login == null || pass == null)
            {
                ErrorMesage = "Неправильно указан паспорт или имя!";
            }
            else
            {
                ErrorMesage = "Неправильный паспорт или имя!";
            }
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            CheckAuth(Login, PassportNumber);
        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the Register window.
            To_Lk_Registred to_Lk_Registred = new To_Lk_Registred();

            // Show the Register window.
            to_Lk_Registred.Show();


        }
    }
}

