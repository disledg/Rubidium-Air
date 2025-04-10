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
        private readonly Dictionary<string, Func<object>> _pages;
        private object _currentPage;

        public NavigationService()
        {
            _pages = new Dictionary<string, Func<object>>();
        }

        public void RegisterPage(string key, Func<object> pageCreator)
        {
            _pages[key] = pageCreator;
        }

        public object NavigateTo(string pageKey)
        {
            if (_pages.TryGetValue(pageKey, out var pageCreator))
            {
                _currentPage = pageCreator();
                CurrentPageChanged?.Invoke();
                return _currentPage;
            }
            throw new ArgumentException($"Page {pageKey} not registered");
        }

        public object CurrentPage => _currentPage;
        public event Action CurrentPageChanged;
    }
}
