using CollegeAdmissionAutomation;
using System.Windows;

namespace Курсач_сайко_1125
{
    public partial class logIn : Window
    {
        private void LoginButton_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Register window.
            logIn registerWindow = new logIn();

            // Show the Register window.
            registerWindow.Show();


        }


    }
}