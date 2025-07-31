using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class SetDeleteStoreCommand : CommandBase
    {
        DeleteStore _store;

        public SetDeleteStoreCommand(DeleteStore store) 
        {
            _store = store;
        }

        /// <summary>
        /// Stages an item to be deleted.
        /// </summary>
        /// <param name="parameter">The item to be staged.</param>
        public override void Execute(object? parameter)
        {
            try
            {
                DomainObject? itemToDelete = parameter as DomainObject;
                _store.ItemToDelete = itemToDelete;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
