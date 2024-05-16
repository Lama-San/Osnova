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

        private ObservableCollection<Zap> _filteredApplicants;
        public ObservableCollection<Zap> FilteredApplicants
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
            LoadZaps();
            SearchCommand = new RelayCommand(SearchApplicants, _ => true);
            CancelSearchCommand = new RelayCommand(CancelSearch, _ => true);
            EnrollCommand = new RelayCommand(Enroll, zap => zap != null);
            NotTodayCommand = new RelayCommand(NotToday, zap => zap != null);
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
                // Log the error or handle it appropriately
                Debug.WriteLine($"Error loading zaps: {ex.Message}");
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterZapsByName(value);
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
                FilteredApplicants = new ObservableCollection<Zap>(Zaps.Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase)));
            }
        }

        private void CancelSearch(object _)
        {
            FilteredApplicants = new ObservableCollection<Zap>(Zaps);
        }

        private void Enroll(object zap)
        {
            if (zap is Zap zapToEnroll)
            {
                // Enroll the zap in the database
                _context.Zaps.Update(zapToEnroll);
                _context.SaveChanges();
            }
        }

        private void NotToday(object zap)
        {
            if (zap is Zap zapToRemove)
            {
                // Remove the zap from the database
                _context.Zaps.Remove(zapToRemove);
                _context.SaveChanges();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}