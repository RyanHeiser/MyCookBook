using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public class RecipeCategoryStore : ChildRecipeStoreBase<RecipeCategory, RecipeBook>
    {
        public RecipeCategoryStore(ChildDataService<RecipeCategory> dataService, RecipeStoreBase<RecipeBook> bookStore) : base(dataService, bookStore)
        {
        }

        /// <summary>
        /// Creates a new RecipeCategory.
        /// </summary>
        /// <param name="category">The RecipeCategory to be created.</param>
        /// <returns>True if created successfully</returns>
        public override async Task<bool> Create(RecipeCategory category)
        {
            if (_parentStore.Current == null)
            {
                return false;
            }
            await _dataService.Create(category);
            _parentStore.Current.AddCategory(category);
            _items.Add(category);

            OnNewCreated(category);
            return true;
        }

        /// <summary>
        /// Deletes a RecipeCategory.
        /// </summary>
        /// <param name="Id">The Id of the RecipeCategory to delete.</param>
        /// <returns>True if deleted successfully.</returns>
        public override async Task<bool> Delete(Guid Id)
        {
            if (_parentStore.Current != null && await _dataService.Delete(Id))
            {
                _parentStore.Current.RemoveCategory(Id);

                RecipeCategory item = _items.FirstOrDefault(c => c.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemDeleted();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a RecipeCategory
        /// </summary>
        /// <param name="Id">The Id of the RecipeCategory to update.</param>
        /// <param name="category">The updated version of the RecipeCategory.</param>
        /// <returns>True if updated successfully</returns>
        public override async Task<bool> Move(RecipeCategory category, Guid newParentId)
        {
            category.ParentId = newParentId;
            if (await _childDataService.Move(category.Id, newParentId))
            {
                _items.Add(category);
                return true;
            }
            return false;
        }

        public override async Task<bool> Update(Guid Id, RecipeCategory category)
        {
            try
            {
                if (_parentStore.Current == null)
                    return false;

                RecipeCategory updatedCategory = await _dataService.Update(Id, category);
                _parentStore.Current.RemoveCategory(Id);
                _parentStore.Current.AddCategory(updatedCategory);

                _items.Add(category);
                RecipeCategory item = _items.FirstOrDefault(c => c.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemUpdated(updatedCategory);
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
