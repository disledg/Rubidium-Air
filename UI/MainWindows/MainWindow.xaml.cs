using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rubidium
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isKeyPressed = false;
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewKeyDown += (s1, e1) => { if (e1.Key == Key.LeftCtrl) isKeyPressed = true; };
            this.PreviewKeyUp += (s2, e2) => { if (e2.Key == Key.LeftCtrl) isKeyPressed = false; };
            this.PreviewMouseLeftButtonDown += (s, e) => { if (isKeyPressed) DragMove(); };

        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
