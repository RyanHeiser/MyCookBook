using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Services.Navigation
{
    public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        /// <summary>
        /// Navigates to the view model specified on initialization.
        /// </summary>
        public void Navigate()
        {
            _navigationStore.Navigate(_createViewModel);
        }
    }
}
