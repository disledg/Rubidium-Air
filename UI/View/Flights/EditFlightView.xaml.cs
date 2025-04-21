using System;
using System.Collections.Generic;
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

namespace Rubidium
{
    /// <summary>
    /// Логика взаимодействия для EditFlightView.xaml
    /// </summary>
    public partial class EditFlightView : Window
    {
        public EditFlightView(Flight flight, FlightService flightService)
        {
            InitializeComponent();
            DataContext = new EditFlightsViewModel(flight, flightService, this);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}