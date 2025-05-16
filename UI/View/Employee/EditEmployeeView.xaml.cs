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
    /// Interaction logic for EditEmployeeView.xaml
    /// </summary>
    public partial class EditEmployeeView : Window
    {
<<<<<<< HEAD
        public EditEmployeeView()
        {
            InitializeComponent();
        }
    }
}
=======
        public EditEmployeeView(Employee employee, EmployeeService employeeService)
        {
            InitializeComponent();
            DataContext = new EditEmployeeViewModel(employee, employeeService, this);
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
>>>>>>> origin/Develop
