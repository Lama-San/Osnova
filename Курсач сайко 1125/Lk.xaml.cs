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
        public LkWindow(int passportNumber, string name)
        {
            InitializeComponent();
            DataContext = this;
            Name = name;
            Gpa = 3.5m; // example value
            Spec = "Computer Science"; // example value
        }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private decimal gpa;
        public decimal Gpa
        {
            get => gpa;
            set
            {
                gpa = value;
                OnPropertyChanged(nameof(Gpa));
            }
        }

        private string spec;
        public string Spec
        {
            get => spec;
            set
            {
                spec = value;
                OnPropertyChanged(nameof(Spec));
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

       

        //private string GetStatus(int passportNumber, string name)
        //{
        //    // Check if the applicant is in Zap, Yeszap, or Nozap tables
        //    // ...
        //    // Return the corresponding status
        //    // ...
        //}
        //private string GetStatus(int passportNumber, string name)
        //{
        //    using (var context = new Dayn1Context())
        //    {
        //        var zap = context.Zaps.FirstOrDefault(z => z.PassportNumber == passportNumber && z.Name == name);
        //        if (zap != null)
        //        {
        //            return "На рассмотрении";
        //        }

        //        var yeszap = context.Yeszaps.FirstOrDefault(y => y.PassportNumber == passportNumber && y.Name == name);
        //        if (yeszap != null)
        //        {
        //            return "Зачислен";
        //        }

        //        var nozap = context.Nozaps.FirstOrDefault(n => n.PassportNumber == passportNumber && n.Name == name);
        //        if (nozap != null)
        //        {
        //            return "Не зачислен";
        //        }

        //        return "Unknown";
        //    }
        //}
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}