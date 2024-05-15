using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Курсач_сайко_1125;
public class Zap : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Gpa { get; set; }
    public string Spec { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
