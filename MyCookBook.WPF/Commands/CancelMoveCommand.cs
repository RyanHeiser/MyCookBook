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

        /// <summary>
        /// Cancels movement of Recipe or RecipeCategory if any is currently being moved.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            _moveStore.IsMoving = false;
        }
    }
}
