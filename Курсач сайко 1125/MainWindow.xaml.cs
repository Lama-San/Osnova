using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CollegeAdmissionAutomation
{
    public partial class MainWindow : Window
    {
       
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
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

        private void CancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.CancelSearchCommand.Execute(null);
            }
        }

        private void applicantsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (applicantsDataGrid.SelectedItem is Applicant applicant)
            {
                MessageBox.Show($"Selected applicant: {applicant.Name} ({applicant.ApplicantID})");
            }
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
        public ICommand CancelSearchCommand { get; private set; }
        private ObservableCollection<Applicant> _filteredApplicants;
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute ?? (o => true);
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute(parameter);
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }
        }

        public MainWindowViewModel()
        {
            Applicants = new ObservableCollection<Applicant>
            {
                new Applicant { ApplicantID = "1234567", Name = "John Doe", GPA = 3.8m },
                new Applicant { ApplicantID = "2345678", Name = "Jane Smith", GPA = 3.9m },
                new Applicant { ApplicantID = "3456789", Name = "Bob Johnson", GPA = 3.5m }

            };

            SearchCommand = new RelayCommand(param => SearchApplicants(""));
            CancelSearchCommand = new RelayCommand(param => CancelSearch());

        }

        public void SearchApplicants(string searchText)
        {
            if (searchText == null)
            {
                throw new ArgumentNullException(nameof(searchText));
            }


            _filteredApplicants = new ObservableCollection<Applicant>(Applicants.Where(a => a.Name.Contains(searchText)));
            Applicants = _filteredApplicants;
        }

        public void CancelSearch()
        {

            Applicants = new ObservableCollection<Applicant>(Applicants.ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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