using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public class RecipeBookStore : RecipeStoreBase<RecipeBook>
    {
        public RecipeBookStore(IDataService<RecipeBook> dataService) : base(dataService)
        {
        }

        public override async Task<bool> Create(RecipeBook book)
        {
            if (await _dataService.Contains(book.Id))
                return false;

            await _dataService.Create(book);
            _items.Add(book);

            OnNewCreated(book);
            return true;
        }

        public override async Task<bool> Delete(Guid Id)
        {
            if (_items.Count > 0 && await _dataService.Delete(Id))
            {
                RecipeBook item = _items.FirstOrDefault(b => b.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemDeleted();
                return true;
            }
            return false;
        }

        public override async Task<bool> Update(Guid Id, RecipeBook book)
        {
            try
            {
                RecipeBook updatedBook = await _dataService.Update(Id, book);

                _items.Add(book);
                RecipeBook item = _items.FirstOrDefault(b => b.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemUpdated(updatedBook);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
