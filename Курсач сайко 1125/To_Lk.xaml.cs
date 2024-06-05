using BD;
using CollegeAdmissionAutomation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using static CollegeAdmissionAutomation.LkWindow;

namespace Курсач_сайко_1125
{
    public partial class To_Lk : Window, INotifyPropertyChanged
    {
        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Data binding properties
        public string PassportNumber { get; set; }
        public string StudentPassword { get; set; }
        public To_Lk()
        {
            InitializeComponent();
            DataContext = this; // Set the DataContext to the current window
        }

        // Error message property for data binding
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                Signal(nameof(ErrorMessage)); // Notify UI of changes
            }
        }

        // Helper function to notify property changes
        void Signal([CallerMemberName] string prop = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void CheckAuth(string pass, string psw)
        {
            var user = DB.Instance.LoginSts.FirstOrDefault(s => s.PassportNumber == pass && s.StudentPassword == psw);
            if (user != null)
            {
                int passportNumber = int.Parse(user.PassportNumber);
                LkWindow lkWindow = new LkWindow(passportNumber, user.StudentName);
                lkWindow.Show();
                this.Close();
            }
            else
            {
                ErrorMessage = "Неправильный паспорт или пароль!";
            }
        }

        private void LoginButton(object sender, RoutedEventArgs e)
        {
            CheckAuth(PassportNumber, passwordBox.Password);
        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            To_Lk_Registred to_Lk_Registred = new To_Lk_Registred();
            to_Lk_Registred.Show();
        }
    }
}