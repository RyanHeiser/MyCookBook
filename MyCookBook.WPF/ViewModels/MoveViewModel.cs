using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
{
    public class MoveViewModel : ViewModelBase
    {
        MoveStore _moveStore;

        public string? Name => _moveStore.Current?.Name;

        public ICommand MoveCommand { get; }
        public ICommand CancelCommand { get; }

        public MoveViewModel(MoveStore moveStore)
        {
            _moveStore = moveStore;

            CancelCommand = new CancelMoveCommand(moveStore);

            _moveStore.MoveUpdated += OnMoveUpdated;
        }

        public override void Dispose()
        {
            _moveStore.MoveUpdated -= OnMoveUpdated;
            base.Dispose();
        }

        private void OnMoveUpdated()
        {
            OnPropertyChanged(nameof(Name));
        }
    }
}
