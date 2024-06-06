using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Курсач_сайко_1125;

namespace CollegeAdmissionAutomation
{
    public partial class LkWindow : Window
    {
        private string PassportNumber;
        public LkWindow(int passportNumber, string name)
        {
            InitializeComponent();
            DataContext = this;
            PassportNumber = passportNumber.ToString();

            using (var context = new Dayn1Context())
            {
                var applicant = context.Loginsts.FirstOrDefault(a => a.PassportNumber == passportNumber.ToString() && a.StudentName == name);
                if (applicant != null)
                {
                    Name = applicant.StudentName;
                    StudentGpa = decimal.TryParse(applicant.StudentGpa, out decimal parsedGpa) ? parsedGpa : 0M;
                    StudentSpec = applicant.StudentSpec;
                    Status = applicant.Status;
                }
                else
                {
                    Name = "Unknown";
                    StudentGpa = 0;
                    StudentSpec = "Unknown";
                    Status = "Неизвестно";
                }
            }

            // Подписываемся на событие StatusChanged из MainViewModel
            if (Application.Current.MainWindow?.DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.StatusChanged += () =>
                {
                    Dispatcher.Invoke(UpdateStatus);
                };
            }
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var dn_Registred = new Dn_Registred(PassportNumber);
            dn_Registred.ShowDialog();
        }
        private string name;
        public string StudentName
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private decimal gpa;
        public decimal StudentGpa
        {
            get => gpa;
            set
            {
                gpa = value;
                OnPropertyChanged();
            }
        }

        private string spec;
        public string StudentSpec
        {
            get => spec;
            set
            {
                spec = value;
                OnPropertyChanged();
            }
        }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private void UpdateStatus()
        {
            using (var context = new Dayn1Context())
            {
                var applicant = context.Loginsts.FirstOrDefault(a => a.PassportNumber == PassportNumber);
                if (applicant != null)
                {
                    Status = applicant.Status;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}