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

        /// <summary>
        /// Creates a new RecipeBook.
        /// </summary>
        /// <param name="book">The RecipeBook to be created.</param>
        /// <returns>True if created successfully</returns>
        public override async Task<bool> Create(RecipeBook book)
        {
            if (await _dataService.Contains(book.Id))
                return false;

            await _dataService.Create(book);
            _items.Add(book);

            OnNewCreated(book);
            return true;
        }

        /// <summary>
        /// Deletes a RecipeBook.
        /// </summary>
        /// <param name="Id">The Id of the RecipeBook to delete.</param>
        /// <returns>True if deleted successfully.</returns>
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

        /// <summary>
        /// Updates a RecipeBook
        /// </summary>
        /// <param name="Id">The Id of the RecipeBook to update.</param>
        /// <param name="book">The updated version of the RecipeBook.</param>
        /// <returns>True if updated successfully</returns>
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
