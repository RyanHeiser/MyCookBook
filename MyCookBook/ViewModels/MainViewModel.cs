using MyCookBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        public ViewModelBase CurrentViewModel => _navigationService.CurrentViewModel;

        public MainViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
