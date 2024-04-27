using CollegeAdmissionAutomation;
using System.Windows;

namespace Курсач_сайко_1125
{
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }
        private void LoginButton_Click(object sender, EventArgs e)
        {          
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Register window.
            Registred registred = new Registred();

            // Show the Register window.
            registred.Show();


        }


    }
}