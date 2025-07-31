using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class DeleteCommand<T> : AsyncCommandBase where T : DomainObject
    {
        private readonly RecipeStoreBase<T> _store;

        public DeleteCommand(RecipeStoreBase<T> store)
        {
            _store = store;
        }

        /// <summary>
        /// Deletes an item.
        /// </summary>
        /// <param name="parameter">The item to delete.</param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is T item)
                await _store.Delete(item.Id);
        }
    }
}
