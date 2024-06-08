using BD;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Курсач_сайко_1125
{
    public partial class To_Lk_Registred : Window, INotifyPropertyChanged
    {
        private Loginst login;
        public int Id { get; set; }

        public string StudentName { get; set; }

        public string StudentPassword { get; set; }

        public string StudentEmail { get; set; }

        public string PassportNumber { get; set; }

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
            if (string.IsNullOrEmpty(txtStudentName.Text) || string.IsNullOrEmpty(passwordBox.Password) || string.IsNullOrEmpty(emailBox.Text) || string.IsNullOrEmpty(textBoxPassport.Text))
            {
                ErrorMessage = "Обязательные поля не заполнены";
                return;
            }

            // Проверяем формат имени
            if (string.IsNullOrWhiteSpace(txtStudentName.Text) || txtStudentName.Text.Contains("(") || txtStudentName.Text.Contains(")") || txtStudentName.Text.EndsWith(" "))
            {
                MessageBox.Show("Неверный формат имени. Имя не должно содержать пробелы, скобки или пробел в конце.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new Dayn1Context())
            {
                // Проверяем, есть ли уже зарегистрированные паспортные данные
                var existingStudent = context.Loginsts.FirstOrDefault(l => l.PassportNumber == textBoxPassport.Text);
                if (existingStudent != null)
                {
                    MessageBox.Show("Такой паспорт уже зарегистрирован");
                    return;
                }

                login = new Loginst
                {
                    StudentName = txtStudentName.Text,
                    StudentPassword = passwordBox.Password,
                    StudentEmail = emailBox.Text,
                    PassportNumber = textBoxPassport.Text,
                    Status = "Неизвестно"
                };

                Id = context.Loginsts.Any() ? context.Loginsts.Max(l => l.Id) + 1 : 1;
                login.Id = Id;
                context.Loginsts.Add(login);
                context.SaveChanges();

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