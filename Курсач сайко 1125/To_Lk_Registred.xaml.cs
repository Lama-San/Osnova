using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Курсач_сайко_1125
{
    public partial class To_Lk_Registred : Window, INotifyPropertyChanged
    {
        private Login login;

        public string FirstName { get; set; }

        private string errorMessage;
        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public To_Lk_Registred()
        {
            InitializeComponent();
            DataContext = this;
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

        private void Check_Register(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(textBoxPassport.Text) || string.IsNullOrEmpty(passwordBox.Password))
            {
                ErrorMessage = "Обязательные поля не заполнены";
                return;
            }
            else
            {
                login = new Login
                {
                    FirstName = txtUsername.Text,                 
                    Password = passwordBox.Password, // получаем значение из passwordBox
                    PassportNumber = textBoxPassport.Text
                };

                using (var context = new Dayn1Context())
                {
                    context.Logins.Add(login);
                    context.SaveChanges();
                }

                MessageBox.Show("Регистрация прошла успешно!");
                this.Close();
            }
        }

        private void btnRegister(object sender, RoutedEventArgs e)
        {
            Check_Register(sender, e);
        }
    }
}