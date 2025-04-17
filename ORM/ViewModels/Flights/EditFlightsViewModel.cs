using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rubidium.ORM.ViewModels.Flights
{
    internal class EditFlightsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public EditFlightsViewModel(Flight flight,
                                  FlightService flightService,
                                  Window window)
        {

        }

        
    }
}
