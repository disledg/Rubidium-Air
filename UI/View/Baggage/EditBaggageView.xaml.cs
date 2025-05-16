<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
=======
﻿using System.Windows;
>>>>>>> origin/Develop
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
<<<<<<< HEAD
    /// Interaction logic for EditBaggageView.xaml
=======
    /// Логика взаимодействия для EditBaggageView.xaml
>>>>>>> origin/Develop
    /// </summary>
    public partial class EditBaggageView : Window
    {
        public EditBaggageView()
        {
            InitializeComponent();
        }
<<<<<<< HEAD
=======

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
>>>>>>> origin/Develop
    }
}
