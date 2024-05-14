using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсач_сайко_1125;
namespace CollegeAdmissionAutomation
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string searchText = "";
        public List<Applicant> _originalApplicants = new List<Applicant>();
        private readonly Login login;
        public List<Zap> Zaps { get; set; }
        public MainWindowViewModel ViewModel { get; set; }
        private ObservableCollection<Applicant> _applicants = new ObservableCollection<Applicant>();
        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        public ObservableCollection<Applicant> Applicants { get => _applicants; set => SetProperty(ref _applicants, value); }

        private void SetProperty(ref ObservableCollection<Applicant> applicants, ObservableCollection<Applicant> value)
        {
            throw new NotImplementedException();
        }
        public MainWindow(Login login)
        {
            InitializeComponent();
            this.login = login;
            DataContext = this;
            
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Close();
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text as string;

            if (searchTerm != null)
            {
                searchTerm = searchTerm.Trim();
            }
            else
            {
                searchTerm = string.Empty;
            }

            try
            {
                ViewModel.SearchApplicants(searchTerm);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При поиске кандидатов произошла ошибка: {ex.Message}", "ААААШИБКА", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CancelSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.CancelSearchCommand.Execute(null);
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
            private Applicant _selectedApplicant;
            public Applicant SelectedApplicant
            {
                get => _selectedApplicant;
                set
                {
                    _selectedApplicant = value;
                    OnPropertyChanged();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public class MainWindowViewModel 
        {
            private ObservableCollection<Applicant> _applicants;
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
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

            private IEnumerable<Applicant> _filteredApplicants;
            public IEnumerable<Applicant> FilteredApplicants
            {
                get => _filteredApplicants;
                set
                {
                    _filteredApplicants = value;
                    OnPropertyChanged();
                }
            }
            public MainWindowViewModel(IEnumerable<Applicant> applicants)
            {
                _applicants = new ObservableCollection<Applicant>(applicants);
                FilteredApplicants = _applicants;

                SearchCommand = new RelayCommand(SearchApplicants, _ => true);
                CancelSearchCommand = new RelayCommand(CancelSearch, _ => true);
            }
            private void SearchApplicants(object searchTerm)
            {
                if (searchTerm is string term)
                {
                    FilteredApplicants = _applicants.Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
                }
            }
            private void CancelSearch(object _)
            {
                FilteredApplicants = _applicants;
            }

            internal void SearchApplicants(string searchTerm)
            {
                throw new NotImplementedException();
            }
        }
        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Predicate<object> _canExecute;

            public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
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

            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }
        }
    } 
}
