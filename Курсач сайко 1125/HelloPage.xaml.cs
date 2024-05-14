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

namespace Курсач_сайко_1125
{

    public partial class HelloPage : Window
    {
        public HelloPage()
        {
            InitializeComponent();

        }
        private void Prepod_click(object sender, RoutedEventArgs e)
        {

            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }
        private void Dn_click (object sender, RoutedEventArgs e)
        {

            Dn_Registred dn_Registred = new Dn_Registred();
            dn_Registred.Show();
        }
    }
}
