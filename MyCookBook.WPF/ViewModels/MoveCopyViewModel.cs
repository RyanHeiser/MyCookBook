using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
{
    public class MoveCopyViewModel : ViewModelBase
    {
        MoveCopyStore _moveCopyStore;

        public ICommand CancelCommand { get; }

        public MoveCopyViewModel(MoveCopyStore moveCopyStore)
        {
            _moveCopyStore = moveCopyStore;
        }
    }
}
