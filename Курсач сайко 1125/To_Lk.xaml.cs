using BD;
using CollegeAdmissionAutomation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using static CollegeAdmissionAutomation.Lk;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Курсач_сайко_1125
{
    public partial class To_Lk : MainWindow, INotifyPropertyChanged
    {
        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get; set; }
        public string PassportNumber { get; set; }
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public To_Lk()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new Dayn1Context());
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
            var user = DB.Instance.Logins.FirstOrDefault(s => s.FirstName == login && s.PassportNumber == pass);
            if (user != null)
            {
                int passportNumber = int.Parse(user.PassportNumber);
                Lk lk = new Lk(passportNumber, user.FirstName);
                new LkWindow(lk).Show(); // Create and show the new window
                Close();
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
            To_Lk_Registred to_Lk_Registred = new To_Lk_Registred();
            to_Lk_Registred.Show();
        }
    }
}

