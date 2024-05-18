using BD;
using Microsoft.VisualBasic.ApplicationServices;
using MySqlConnector;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Курсач_сайко_1125;
using Microsoft.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Diagnostics;
using CollegeAdmissionAutomation.ViewModels;
//то что надо
namespace CollegeAdmissionAutomation
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new Dayn1Context());
        }
        
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            new LogIn().Show();
            Close();
        }
        private void TheBlessedOnes(object sender, RoutedEventArgs e)
        {
            TheBlessedOnes theBlessedOnes = new TheBlessedOnes((MainViewModel)DataContext);
            theBlessedOnes.Show();
        }
        private void Zachislit(object sender, RoutedEventArgs e)
        {
            if (SelectedZap != null)
            {
                using (var context = new Dayn1Context())
                {
                    var yeszap = new Yeszap
                    {
                        Name = SelectedZap.Name,
                        Gpa = SelectedZap.Gpa,
                        Spec = SelectedZap.Spec
                    };

                    context.Yeszaps.Add(yeszap);
                    context.SaveChanges();
                }

                SelectedZap = null;
            }
        }


        public List<Zap> zap { get; set; }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if (SelectedZap != null)
            {
                using (var context = new Dayn1Context())
                {
                    var zap = context.Zaps.Find(SelectedZap.Id);
                    if (zap != null)
                    {
                        context.Zaps.Remove(zap);
                        context.SaveChanges();
                    }
                }

                SelectedZap = null;
            }
        }

        private Zap selectedZap;

        public Zap SelectedZap
        {
            get => selectedZap;
            set
            {
                selectedZap = value;
               
            }
        }
    }

    public class Zap : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private decimal gpa;
        public decimal Gpa
        {
            get => gpa;
            set
            {
                if (gpa != value)
                {
                    gpa = value;
                    OnPropertyChanged(nameof(Gpa));
                }
            }
        }

        

        private string spec;
        public string Spec
        {
            get => spec;
            set
            {
                if (spec != value)
                {
                    spec = value;
                    OnPropertyChanged(nameof(Spec));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void Signal([CallerMemberName] string prop = null) =>
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Dayn1Context _context;
    private string _searchText = "";

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<Zap> Zaps { get; private set; }
    public ICommand SearchCommand { get; private set; }
    public ICommand CancelSearchCommand { get; private set; }

    private IEnumerable<Zap> _filteredApplicants;
    public IEnumerable<Zap> FilteredApplicants
    {
        get => _filteredApplicants;
        set
        {
            _filteredApplicants = value;
            OnPropertyChanged(nameof(FilteredApplicants));
        }
    }

    public MainViewModel(Dayn1Context context)
    {
        _context = context;
        Zaps = new ObservableCollection<Zap>(context.Zaps.ToList());
        FilteredApplicants = Zaps;

        SearchCommand = new RelayCommand(SearchApplicants, _ => true);
        CancelSearchCommand = new RelayCommand(CancelSearch, _ => true);
    }

    private void SearchApplicants(object searchTerm)
    {
        if (searchTerm is string term)
        {
            FilteredApplicants = Zaps.Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
    }

    private void CancelSearch(object _)
    {
        FilteredApplicants = Zaps;
    }

    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
