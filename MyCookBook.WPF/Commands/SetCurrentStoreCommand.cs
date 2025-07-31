using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class SetCurrentStoreCommand<T> : CommandBase where T : DomainObject
    {
        private readonly RecipeStoreBase<T> _store;

        public SetCurrentStoreCommand(RecipeStoreBase<T> store)
        {
            _store = store;
        }

        /// <summary>
        /// Sets the item store's Current property to specified item.
        /// </summary>
        /// <param name="parameter">The item to set as Current.</param>
        public override void Execute(object? parameter)
        {
            try
            {
                T? item = parameter as T;
                _store.Current = item;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
