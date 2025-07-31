using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Services.Navigation
{
    class PreviousNavigationService : INavigationService
    {
        private readonly NavigationStore _navigationStore;

        public PreviousNavigationService(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        /// <summary>
        /// Navigates to the previous view model.
        /// </summary>
        public void Navigate()
        {
            _navigationStore.NavigatePrevious();
        }
    }
}
