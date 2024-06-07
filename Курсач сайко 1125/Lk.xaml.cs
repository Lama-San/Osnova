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
            StatusText = UpdateStatus();

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
        private string statusText;
        public string StatusText
        {
            get => statusText;
            set
            {
                if (statusText != value)
                {
                    statusText = value;
                    OnPropertyChanged(nameof(StatusText));
                }
            }
        }
        private string UpdateStatus()
        {
            using (var context = new Dayn1Context())
            {
                var inYeszap = context.Yeszaps.Any(y => y.PassportNumber == PassportNumber);
                var inNozap = context.Nozaps.Any(n => n.PassportNumber == PassportNumber);
                var inZap = context.Zaps.Any(n => n.PassportNumber == PassportNumber);

                if (inYeszap)
                {
                    Status = "Зачислен";
                    return "Зачислен";
                }
                else if (inNozap)
                {
                    Status = "Не принят";
                    return "Не принят";
                }

                else if (inZap)
                {
                    Status = "На рассмотрении"; 
                    return "На рассмотрении";
                }

                return "Неизвестно"; 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}