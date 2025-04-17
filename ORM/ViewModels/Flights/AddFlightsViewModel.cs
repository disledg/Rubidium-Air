using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rubidium.ORM.ViewModels.Flights
{
    internal class AddFlightsViewModel : INotifyPropertyChanged
    {
        public AddFlightsViewModel(FlightsViewModel parentViewModel,
                                 FlightService flightService,
                                 Window window)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
