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
            LoadData(passportNumber, name);
            PassportNumber = passportNumber.ToString(); // Сохранить номер паспорта как строку для передачи в Dn_Registred
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
                OnPropertyChanged(nameof(Name));
            }
        }

        private decimal gpa;
        public decimal StudentGpa
        {
            get => gpa;
            set
            {
                gpa = value;
                OnPropertyChanged(nameof(StudentGpa));
            }
        }

        private string spec;
        public string StudentSpec
        {
            get => spec;
            set
            {
                spec = value;
                OnPropertyChanged(nameof(StudentSpec));
            }
        }

        private string status;
        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private void LoadData(int passportNumber, string name)
        {
            using (var context = new Dayn1Context())
            {
                Status = GetStatus(passportNumber, name);
                var applicant = context.Loginsts.FirstOrDefault(a => a.PassportNumber == passportNumber.ToString() && a.StudentName == name);
                if (applicant != null)
                {
                    Name = applicant.StudentName;
                    StudentGpa = decimal.TryParse(applicant.StudentGpa, out decimal parsedGpa) ? parsedGpa : 0M;
                    StudentSpec = applicant.StudentSpec; // Assuming Specialization exists in LoginSt
                }
                else
                {
                    // Handle the case where the applicant does not exist in the database.
                    // You could set default values or show an error message.
                    Name = "Unknown";
                    StudentGpa = 0;
                    StudentSpec = "Unknown";
                }
            }
        }

        private string GetStatus(int passportNumber, string name)
        {
            using (var context = new Dayn1Context())
            {
                if (context.Zaps.Any(z => z.PassportNumber == passportNumber.ToString() && z.Name == name))
                {
                    return "На рассмотрении";
                }
                else if (context.Yeszaps.Any(y => y.PassportNumber == passportNumber.ToString() && y.Name == name))
                {
                    return "Зачислен";
                }
                else if (context.Nozaps.Any(n => n.PassportNumber == passportNumber.ToString() && n.Name == name))
                {
                    return "Не зачислен";
                }
                else
                {
                    return "Неизвестно";
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