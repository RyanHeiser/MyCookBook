using MyCookBook.WPF.Services;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly MoveCopyStore _moveCopyStore;
        private readonly Func<MoveCopyViewModel> _moveCopyVmFactory;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
        public ViewModelBase MoveCopyViewModel => _moveCopyVmFactory();
        public bool IsModalOpen => _modalNavigationStore.IsOpen;
        public bool IsMoving => _moveCopyStore.IsMoving || _moveCopyStore.IsCopying;

        public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, MoveCopyStore moveCopyStore, Func<MoveCopyViewModel> moveCopyVmFactory)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;
            _moveCopyStore = moveCopyStore;
            _moveCopyVmFactory = moveCopyVmFactory;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
            _moveCopyStore.MoveCopyStarted += OnMoveCopyStarted;
        }

        private void OnMoveCopyStarted()
        {
            OnPropertyChanged(nameof(IsMoving));
            OnPropertyChanged(nameof(MoveCopyViewModel));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsModalOpen));
        }
    }
}
