using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public class RecipeImageStore : ChildRecipeStoreBase<RecipeImage, Recipe>
    {
        public RecipeImageStore(ChildDataService<RecipeImage> dataService, RecipeStoreBase<Recipe> recipeStore) : base(dataService, recipeStore) 
        {
        }

        /// <summary>
        /// Creates a new RecipeImage.
        /// </summary>
        /// <param name="image">The RecipeImage to be created.</param>
        /// <returns>True if created successfully</returns>
        public override async Task<bool> Create(RecipeImage image)
        {
            if (_parentStore.Current == null)
            {
                return false;
            }
            await _dataService.Create(image);
            _items.Add(image);

            OnNewCreated(image);
            return true;
        }

        /// <summary>
        /// Deletes a RecipeImage.
        /// </summary>
        /// <param name="Id">The Id of the RecipeImage to delete.</param>
        /// <returns>True if deleted successfully.</returns>
        public override async Task<bool> Delete(Guid Id)
        {
            if (_parentStore.Current != null && await _dataService.Delete(Id))
            {
                //_parentStore.Current.RemoveRecipe(Id);

                RecipeImage item = _items.FirstOrDefault(i => i.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemDeleted();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a RecipeImage
        /// </summary>
        /// <param name="Id">The Id of the RecipeImage to update.</param>
        /// <param name="image">The updated version of the RecipeImage.</param>
        /// <returns>True if updated successfully</returns>
        public override async Task<bool> Update(Guid Id, RecipeImage image)
        {
            try
            {
                if (_parentStore.Current == null)
                    return false;

                RecipeImage updatedImage = await _dataService.Update(Id, image);
                //_parentStore.Current.RemoveRecipe(Id);
                //_parentStore.Current.AddRecipe(updatedRecipe);

                _items.Add(image);
                RecipeImage item = _items.FirstOrDefault(i => i.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemUpdated(updatedImage);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public override async Task<bool> Move(RecipeImage item, Guid newParentId)
        {
            return false;
        }
    }
}
