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
            
            new HelloPage().Show();          
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
        public event Action StatusChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand EnrollCommand { get; private set; } // Команда для зачисления
        public ICommand RemoveCommand { get; private set; } // Команда для удаления
        public ICommand NotTodayCommand { get; private set; } // Команда для переноса в Nozap
        public IQueryable<Zap> Zaps { get; private set; } // Запросы к таблице Zap
        public IQueryable<Yeszap> Yeszaps { get; private set; } // Запросы к таблице Yeszap
        public ICommand OpenTheBlessedOnesCommand { get; private set; } // Команда для открытия окна "Зачисленные"
        public ICommand OpenLosersCommand { get; private set; } // Команда для открытия окна "Неудачники"

        private Zap? selectedApplicant; // Выбранный кандидат
        private ObservableCollection<Zap> filteredApplicants; // Отфильтрованные кандидаты
        private ObservableCollection<Yeszap> yeszap; // Зачисленные кандидаты
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
            LoadZaps(); // Загружаем список кандидатов
            LoadYeszaps(); // Загружаем список зачисленных
            Yeszap = new ObservableCollection<Yeszap>();
            OpenLosersCommand = new RelayCommand(OnOpenLosers, _ => true);// Создаем команду для открытия окна "Неудачники"
            OpenTheBlessedOnesCommand = new RelayCommand(OnOpenTheBlessedOnes, _ => true); // Создаем команду для открытия окна "Зачисленные"
            EnrollCommand = new RelayCommand(param => EnrollApplicant(), _ => SelectedApplicant != null); // Создаем команду для зачисления
            NotTodayCommand = new RelayCommand(param => NotToday(), canExecute: _ => SelectedApplicant != null); // Создаем команду для переноса в Nozap
        }
        private async void EnrollApplicant()
        {
            if (SelectedApplicant != null && !string.IsNullOrEmpty(SelectedApplicant.Name))
            {
                var loginstToUpdate = context.Loginsts.FirstOrDefault(l => l.StudentName == SelectedApplicant.Name);
                if (loginstToUpdate != null)
                {
                    loginstToUpdate.Status = "Зачислен";
                    try
                    {
                        await context.SaveChangesAsync();
                        StatusChanged?.Invoke(); // Вызов события после успешного сохранения изменений
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }

                var yeszap = new Yeszap
                {
                    Name = SelectedApplicant.Name,
                    Gpa = SelectedApplicant.Gpa,
                    Spec = SelectedApplicant.Spec
                };
                context.Yeszaps.Add(yeszap);
                await context.SaveChangesAsync();

                var zapToRemove = context.Zaps.FirstOrDefault(s => s.Id == SelectedApplicant.Id);
                if (zapToRemove != null)
                {
                    context.Zaps.Remove(zapToRemove);
                    await context.SaveChangesAsync();
                }
                StatusChanged?.Invoke();
                MessageBox.Show("Неужели у погасшей души будет шанс?");
                SelectedApplicant = null;
                await LoadZaps();
                await LoadYeszaps();
            }
        }


        private async void NotToday()
        {
            if (SelectedApplicant != null && !string.IsNullOrEmpty(SelectedApplicant.Name))
            {
                var loginstToUpdate = context.Loginsts.FirstOrDefault(l => l.StudentName == SelectedApplicant.Name);
                if (loginstToUpdate != null)
                {
                    loginstToUpdate.Status = "Не принят";
                    try
                    {
                        await context.SaveChangesAsync();
                        StatusChanged?.Invoke(); // Вызов события после успешного сохранения изменений
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }

                var nozap = new Nozap
                {
                    Name = SelectedApplicant.Name,
                    Gpa = SelectedApplicant.Gpa,
                    Spec = SelectedApplicant.Spec
                };
                context.Nozaps.Add(nozap);
                await context.SaveChangesAsync();

                var zapToRemove = context.Zaps.FirstOrDefault(s => s.Id == SelectedApplicant.Id);
                if (zapToRemove != null)
                {
                    context.Zaps.Remove(zapToRemove);
                    try
                    {
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении изменений: {ex.Message}");
                    }
                }

                StatusChanged?.Invoke();
                MessageBox.Show("За что вы так с ним, он хотел жить...");
                SelectedApplicant = null;
                await LoadZaps();
            }
        }
        private async Task LoadZaps()
        {
            // Обработка исключений
            try
            {
                // Загружаем список кандидатов из базы данных
                var zaps = await context.Zaps.ToListAsync();
                // Преобразуем список кандидатов в запросы
                Zaps = zaps.Select(MapZap).AsQueryable();
                // Создание списка отфильтрованных кандидатов
                FilteredApplicants = new ObservableCollection<Zap>(Zaps);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибкка загрузки zaps", ex);
            }
        }
        private async Task LoadYeszaps()
        {
            // Обработка исключений
            try
            {
                // Загружаем список зачисленных кандидатов из базы данных
                var yeszaps = await context.Yeszaps.ToListAsync();
                // Преобразуем список зачисленных кандидатов в запросы
                Yeszaps = yeszaps.AsQueryable();
                // Создаем список зачисленных кандидатов
                Yeszap = new ObservableCollection<Yeszap>(Yeszaps);
                // Обновляем уведомление о изменении свойства
                OnPropertyChanged(nameof(Yeszap));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка загрузки yeszaps", ex);
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
            // Если текст поиска пуст, то показываем весь список кандидатов
            if (string.IsNullOrEmpty(searchText))
            {
                FilteredApplicants = new ObservableCollection<Zap>(Zaps);
            }
            else
            {
                // Фильтруем список кандидатов по имени или специальности
                FilteredApplicants = new ObservableCollection<Zap>(Zaps.Where(z => z.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) || z.Spec.Contains(searchText, StringComparison.OrdinalIgnoreCase)));
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
        private void OnOpenLosers(object _)
        {
            // Здесь загружаются данные из Nozap и передаете их в окно Losers.
            var nozaps = context.Nozaps.ToList();
            var losers = new Losers(new ObservableCollection<Nozap>(nozaps));
            losers.Show();
        }
        private void OnOpenTheBlessedOnes(object _)
        {
            // Создаем новое окно "Зачисленные" и показываем его
            var theBlessedOnes = new TheBlessedOnes(Yeszap);
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