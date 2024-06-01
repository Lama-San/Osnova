using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace Курсач_сайко_1125
{
    public partial class Losers : Window
    {
        public ObservableCollection<Nozap> Nozaps { get; set; }

        public Losers(ObservableCollection<Nozap> nozaps)
        {
            InitializeComponent();
            Nozaps = nozaps;
            DataContext = this; // Устанавливаем DataContext для привязки
        }
    }
}