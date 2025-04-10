using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rubidium
{
    public class NavigationService
    {

        private Frame _frame;

        public void Initialize(Frame frame) => _frame = frame;
        public void NavigateTo(Page page) => _frame?.Navigate(page);
    }
}
