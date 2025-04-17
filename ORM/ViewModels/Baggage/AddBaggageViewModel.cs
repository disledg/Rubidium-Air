using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rubidium
{
    internal class AddBaggageViewModel : INotifyPropertyChanged
    {
        public AddBaggageViewModel(BaggageViewModel parentViewModel,
                                 BaggageService baggageService,
                                 Window window)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
