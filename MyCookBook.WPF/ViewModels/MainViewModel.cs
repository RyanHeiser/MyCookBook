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
        private readonly MoveStore _moveStore;
        private readonly Func<MoveViewModel> _moveVmFactory;

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;
        public ViewModelBase? CurrentModalViewModel => _modalNavigationStore.CurrentViewModel;
        public ViewModelBase? MoveViewModel { get; private set; }

        public bool IsModalOpen => _modalNavigationStore.IsOpen;
        public bool IsMoving => _moveStore.IsMoving;

        public MainViewModel(NavigationStore navigationStore, ModalNavigationStore modalNavigationStore, MoveStore moveStore, Func<MoveViewModel> moveVmFactory)
        {
            _navigationStore = navigationStore;
            _modalNavigationStore = modalNavigationStore;
            _moveStore = moveStore;
            _moveVmFactory = moveVmFactory;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
            _moveStore.MoveUpdated += OnMoveUpdated;
            _moveStore.MoveStarted += OnMoveStarted;
        }

        private void OnMoveUpdated()
        {
            OnPropertyChanged(nameof(IsMoving));
        }

        private void OnMoveStarted()
        {
            MoveViewModel?.Dispose();
            MoveViewModel = _moveVmFactory();
            OnPropertyChanged(nameof(MoveViewModel));
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
