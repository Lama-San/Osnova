using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace CollegeAdmissionAutomation
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;
            
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            if (ViewModel != null)
            {
                ViewModel.SearchApplicants(searchTerm);
            }
        }
      

        public class Applicant : INotifyPropertyChanged
        {
            private string _applicantID;
            private string _name;
            private decimal _gpa;

            public string ApplicantID
            {
                get => _applicantID;
                set
                {
                    _applicantID = value;
                    OnPropertyChanged();
                }
            }

            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }

            public decimal GPA
            {
                get => _gpa;
                set
                {
                    _gpa = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class MainWindowViewModel : INotifyPropertyChanged
        {
            private ObservableCollection<Applicant> _applicants;
            public ObservableCollection<Applicant> Applicants
            {
                get => _applicants;
                set
                {
                    _applicants = value;
                    OnPropertyChanged();
                }
            }

            public ICommand SearchCommand { get; private set; }
            public IEnumerable<Applicant> filteredApplicants { get; private set; }

            public MainWindowViewModel()
            {
                Applicants = new ObservableCollection<Applicant>
                {
                    new Applicant { ApplicantID = "1234567", Name = "John Doe", GPA = 3.8m },
                    new Applicant { ApplicantID = "2345678", Name = "Jane Smith", GPA = 3.9m },
                    new Applicant { ApplicantID = "3456789", Name = "Bob Johnson", GPA = 3.5m }
                };

                SearchCommand = new RelayCommand(param => SearchApplicants(""));
            }

            public void SearchApplicants(string searchText)
            {
                // Filter the applicants based on the search text
                // ...

                // Set the filtered applicants as the new value for the Applicants property
                Applicants = new ObservableCollection<Applicant>(filteredApplicants);
                OnPropertyChanged();
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class RelayCommand : ICommand
        {
            private Action<object> _execute;
            private Func<object, bool> _canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }
    }
}
//private void SearchButton_Click(object sender, RoutedEventArgs e)
//{
//    string searchTerm = searchTextBox.Text.Trim();
//    if (!string.IsNullOrEmpty(searchTerm))
//    {
//        DataView applicantsView = applicantsDataTable.DefaultView;
//        applicantsView.RowFilter = $"Name LIKE '%{searchTerm}%'";
//        applicantsDataGrid.ItemsSource = applicantsView;
//    }
//    else
//    {
//        LoadApplicants();
//        applicantsDataGrid.ItemsSource = applicantsDataTable.DefaultView;
//    }
//}