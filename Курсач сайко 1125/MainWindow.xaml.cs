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

        public class MainViewModel : INotifyPropertyChanged
        {
            private Dayn1Context context;
            private string searchText = "";

            public event PropertyChangedEventHandler PropertyChanged;
            public ICommand EnrollCommand { get; private set; }
            public ICommand RemoveCommand { get; private set; }
            public IQueryable<Zap> Zaps { get; private set; }
            public ICommand SearchCommand { get; private set; }
            public ICommand CancelSearchCommand { get; private set; }
            public ICommand OpenTheBlessedOnesCommand { get; private set; }

            private Zap? selectedApplicant;
            private ObservableCollection<Zap> filteredApplicants;
            private ObservableCollection<Yeszap> yeszap;
            public ObservableCollection<Zap> FilteredApplicants
            {
                get => filteredApplicants;
                set
                {
                    filteredApplicants = value;
                    OnPropertyChanged(nameof(FilteredApplicants));
                }
            }

            public ObservableCollection<Yeszap> Yeszap
            {
                get => yeszap;
                set
                {
                    yeszap = value;
                    OnPropertyChanged(nameof(Yeszap));
                }
            }

            public Zap? SelectedApplicant
            {
                get => selectedApplicant;
                set
                {
                    selectedApplicant = value;
                    OnPropertyChanged(nameof(SelectedApplicant));
                }
            }
            public MainViewModel(Dayn1Context context)
            {
                this.context = context;
                LoadZaps();
                SearchCommand = new RelayCommand(SearchApplicants, _ => true);
                CancelSearchCommand = new RelayCommand(CancelSearch, _ => true);
                Yeszap = new ObservableCollection<Yeszap>();
                OpenTheBlessedOnesCommand = new RelayCommand(OnOpenTheBlessedOnes, _ => true);
                EnrollCommand = new RelayCommand(param => EnrollApplicant(), _ => SelectedApplicant != null);
                RemoveCommand = new RelayCommand(param => RemoveApplicant(), canExecute: _ => SelectedApplicant != null);
            }
            private async void EnrollApplicant()
            {
                if (SelectedApplicant != null)
                {
                    // Use the _context from the view model, not a new instance
                    var yeszap = new Yeszap
                    {
                        Name = SelectedApplicant.Name,
                        Gpa = SelectedApplicant.Gpa,
                        Spec = SelectedApplicant.Spec
                    };

                    context.Yeszaps.Add(yeszap);
                    context.SaveChanges();
                    context = new Dayn1Context();

                    var zapToRemove = context.Zaps.FirstOrDefault(s => s.Id == SelectedApplicant.Id);
                    if (zapToRemove != null)
                    {
                        var res = context.Zaps.Where(s => s.Id == zapToRemove.Id).ExecuteDelete();

                        if (res == 0) return;
                    }

                    await context.SaveChangesAsync();

                    SelectedApplicant = null;
                    await LoadZaps(); // Reload the data after the change
                }

            }

            private async void RemoveApplicant()
            {
                try
                {
                    if (SelectedApplicant != null)
                    {
                        var zapToRemove = context.Zaps.Find(SelectedApplicant.Id);
                        if (zapToRemove != null)
                        {
                            context.Zaps.Remove(zapToRemove);
                            await context.SaveChangesAsync();
                        }

                        SelectedApplicant = null;
                        await LoadZaps(); // Reload the data after the change
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Database not save");
                }
            }
            private async Task LoadZaps()
            {
                try
                {
                    var zaps = await context.Zaps.ToListAsync();
                    Zaps = zaps.Select(MapZap).AsQueryable();
                    FilteredApplicants = new ObservableCollection<Zap>(Zaps);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error loading zaps", ex);
                }
            }

            public string SearchText
            {
                get => searchText;
                set
                {
                    searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterZapsByName(searchText);
                }
            }
            private void FilterZapsByName(string searchText)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    FilteredApplicants = new ObservableCollection<Zap>(Zaps);
                }
                else
                {
                    FilteredApplicants = new ObservableCollection<Zap>(Zaps.Where(z => z.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
                }
            }

            CollegeAdmissionAutomation.Zap MapZap(Курсач_сайко_1125.Zap zap)
            {
                return new CollegeAdmissionAutomation.Zap
                {
                    Id = zap.Id,
                    Name = zap.Name,
                    Gpa = zap.Gpa,
                    Spec = zap.Spec
                };
            }

            private void SearchApplicants(object searchTerm)
            {
                if (searchTerm is string term)
                {
                    SearchText = term;
                }
            }

            private void CancelSearch(object _)
            {
                FilteredApplicants = new ObservableCollection<Zap>(Zaps);
            }

            private void OnOpenTheBlessedOnes(object _)
            {
                var theBlessedOnes = new TheBlessedOnes(context);
                theBlessedOnes.Show();
            }

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            public class RelayCommand : ICommand
            {
                private readonly Action<object> execute;
                private readonly Func<object, bool> canExecute;
            

                public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
                {
                    this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
                    this.canExecute = canExecute;
                }

           
            
                public bool CanExecute(object parameter)
                {
                    return canExecute == null || canExecute(parameter);
                }

                public void Execute(object parameter)
                {
                    execute(parameter);
                }

                public event EventHandler CanExecuteChanged
                {
                    add { CommandManager.RequerySuggested += value; }
                    remove { CommandManager.RequerySuggested -= value; }
                }
            }
        }

    
    }


