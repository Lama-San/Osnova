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
        public string Login { get; set; }
        public string PassportNumber { get; set; }

        public To_Lk()
        {
            InitializeComponent();
            // DataContext = new MainViewModel(new Dayn1Context()); // Initialize your view model if needed
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

        private void CheckAuth(string login, string pass)
        {
            var user = DB.Instance.Logins.FirstOrDefault(s => s.FirstName == login && s.PassportNumber == pass);
            if (user != null)
            {
                int passportNumber = int.Parse(user.PassportNumber);
                LkWindow lkWindow = new LkWindow(passportNumber, user.FirstName);
                lkWindow.Show();
                this.Close();
            }
            else
            {
                ErrorMessage = "Неправильный паспорт или имя!";
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