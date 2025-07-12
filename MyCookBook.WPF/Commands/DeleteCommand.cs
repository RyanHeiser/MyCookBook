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
        private readonly T? _itemToDelete;

        public DeleteCommand(RecipeStoreBase<T> store)
        {
            _store = store;
        }
        
        //public DeleteCommand(RecipeStoreBase<T> store, T itemToDelete)
        //{
        //    _store = store;
        //    _itemToDelete = itemToDelete;
        //}

        public override async Task ExecuteAsync(object? parameter)
        {
            //if (_itemToDelete != null)
            //{
            //    await _store.Delete(_itemToDelete.Id);
            //    return;
            //}

            try
            {
                T? item = parameter as T;
                await _store.Delete(item.Id);
            }
            catch (Exception)
            {
                throw new InvalidCastException();
            }
        }
    }
}
