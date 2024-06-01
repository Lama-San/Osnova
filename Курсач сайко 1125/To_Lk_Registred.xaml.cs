using BD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class To_Lk_Registred : Window
    {
        private Login login;

        //public string PassportNumber { get; set; }
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
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(textBoxPasspotr.Text))
            {
                ErrorMessage = "Обязательные поля не заполнены";
                return;
            }
            else
            {
                var login = new Login
                {
                    FirstName = txtUsername.Text,
                    PassportNumber = textBoxPasspotr.Text, // получаем значение из textBoxPasspotr
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