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
using Курсач_сайко_1125.Models;

namespace CollegeAdmissionAutomation.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Dayn1Context _context;
        private string _searchText = "";

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand EnrollCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public IQueryable<Zap> Zaps { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand CancelSearchCommand { get; private set; }
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
            EnrollCommand = new RelayCommand(async _ => await EnrollApplicant(null), _ => SelectedApplicant != null);
            RemoveCommand = new RelayCommand(async _ => await RemoveApplicant(null), _ => SelectedApplicant != null);
        }
        private async Task EnrollApplicant(object _)
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

                _context.Yeszaps.Add(yeszap);
                _context.SaveChanges();
                _context = new Dayn1Context();

                var zapToRemove = _context.Zaps.FirstOrDefault(s => s.Id == SelectedApplicant.Id);
                if (zapToRemove != null)
                {
                    var res = _context.Zaps.Where(s => s.Id == zapToRemove.Id).ExecuteDelete();

                    if (res == 0) return;
                }

                await _context.SaveChangesAsync();

                SelectedApplicant = null;
                await LoadZaps(); // Reload the data after the change
            }

        }
        private async Task RemoveApplicant(object _)
        {
            if (SelectedApplicant != null)
            {
                var zapToRemove = _context.Zaps.Find(SelectedApplicant.Id);
                if (zapToRemove != null)
                {
                    _context.Zaps.Remove(zapToRemove);
                    await _context.SaveChangesAsync();
                }

                SelectedApplicant = null;
                await LoadZaps(); // Reload the data after the change
            }
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

        
        CollegeAdmissionAutomation.Zap MapZap(Курсач_сайко_1125.Models.Zap zap)
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