using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CancelMoveCommand : CommandBase
    {
        private readonly MoveStore _moveStore;

        public CancelMoveCommand(MoveStore moveStore)
        {
            _moveStore = moveStore;
        }

        public override void Execute(object? parameter)
        {
            _moveStore.IsMoving = false;
        }
    }
}
