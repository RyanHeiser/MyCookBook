﻿using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Services.Navigation
{
    public class ModalNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public ModalNavigationService(ModalNavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        /// <summary>
        /// Navigates to the modal view model specified on initialization.
        /// </summary>
        public void Navigate()
        {
            _navigationStore.Navigate(_createViewModel);
        }
    }
}
