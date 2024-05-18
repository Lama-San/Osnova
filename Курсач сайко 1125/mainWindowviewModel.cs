using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CollegeAdmissionAutomation.ViewModels;
using Microsoft.EntityFrameworkCore;
using static CollegeAdmissionAutomation.MainWindow;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Курсач_сайко_1125;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows;

namespace CollegeAdmissionAutomation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Dayn1Context _context;
        private string _searchText = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public IQueryable<Zap> Zaps { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand CancelSearchCommand { get; private set; }
        public ICommand EnrollCommand { get; private set; }
        public ICommand NotTodayCommand { get; private set; }
        public ICommand OpenTheBlessedOnesCommand { get; private set; }


        private Zap? _selectedApplicant;
        private ObservableCollection<Zap> _filteredApplicants;
        private ObservableCollection<Yeszap> _yeszap;
        public ObservableCollection<Zap> FilteredApplicants
        {
            get => _filteredApplicants;
            set
            {
                _filteredApplicants = value;
                OnPropertyChanged(nameof(FilteredApplicants));
            }
        }
        public ObservableCollection<Yeszap> Yeszap
        {
            get => _yeszap;
            set
            {
                _yeszap = value;
                OnPropertyChanged(nameof(Yeszap));
            }
        }

        public Zap? SelectedApplicant
        {
            get => _selectedApplicant;
            set
            {
                _selectedApplicant = value;
                OnPropertyChanged(nameof(SelectedApplicant));
            }
        }
        public MainViewModel(Dayn1Context context)
        {
            _context = context;
            LoadZaps();
            SearchCommand = new RelayCommand(SearchApplicants, _ => true);
            CancelSearchCommand = new RelayCommand(CancelSearch, _ => true);
            Yeszap = new ObservableCollection<Yeszap>();          
            OpenTheBlessedOnesCommand = new RelayCommand(OnOpenTheBlessedOnes, _ => true);
        }

         private async Task LoadZaps()
        {
            try
            {
                Zaps = _context.Zaps.Select(MapZap).AsQueryable();
                FilteredApplicants = new ObservableCollection<Zap>(await Zaps.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error loading zaps", ex);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredApplicants = new ObservableCollection<Zap>(Zaps.Where(z => z.Name.Contains(value, StringComparison.OrdinalIgnoreCase)));
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

        private CollegeAdmissionAutomation.Zap MapZap(Курсач_сайко_1125.Zap zap)
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
            var theBlessedOnes = new TheBlessedOnes(_context);
            theBlessedOnes.Show();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}