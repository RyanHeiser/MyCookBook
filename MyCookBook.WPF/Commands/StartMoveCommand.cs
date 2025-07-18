using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class StartMoveCommand<T> : CommandBase where T : ChildDomainObject
    {
        private readonly MoveStore _moveCopyStore;
        private T? _itemToMove;

        public StartMoveCommand(MoveStore moveCopyStore)
        {
            _moveCopyStore = moveCopyStore;
        }

        public StartMoveCommand(MoveStore moveCopyStore, T? itemToMove)
        {
            _moveCopyStore = moveCopyStore;
            _itemToMove = itemToMove;
        }

        public override void Execute(object? parameter)
        {
            if (_itemToMove == null)
            {
                if (parameter is not T) return;

                _itemToMove = parameter as T;
            }

            _moveCopyStore.Current = _itemToMove;
            _moveCopyStore.IsMoving = true;
        }
    }
}
