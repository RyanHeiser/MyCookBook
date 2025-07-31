using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores.RecipeStores
{
    public class RecipeStore : ChildRecipeStoreBase<Recipe, RecipeCategory>
    {
        public RecipeStore(ChildDataService<Recipe> dataService, RecipeStoreBase<RecipeCategory> categoryStore) : base(dataService, categoryStore)
        {
        }

        /// <summary>
        /// Creates a new Recipe.
        /// </summary>
        /// <param name="recipe">The Recipe to be created.</param>
        /// <returns>True if created successfully</returns>
        public override async Task<bool> Create(Recipe recipe)
        {
            if (_parentStore.Current == null)
            {
                return false;
            }
            await _dataService.Create(recipe);
            _items.Add(recipe);
            _parentStore.Current.AddRecipe(recipe);

            UpdateParent();
            OnNewCreated(recipe);
            return true;
        }

        /// <summary>
        /// Deletes a Recipe.
        /// </summary>
        /// <param name="Id">The Id of the Recipe to delete.</param>
        /// <returns>True if deleted successfully.</returns>
        public override async Task<bool> Delete(Guid Id)
        {
            if (_parentStore.Current != null && await _dataService.Delete(Id))
            {
                _parentStore.Current.RemoveRecipe(Id);

                Recipe item = _items.FirstOrDefault(r => r.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemDeleted();
                UpdateParent();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Updates a Recipe
        /// </summary>
        /// <param name="Id">The Id of the Recipe to update.</param>
        /// <param name="recipe">The updated version of the Recipe.</param>
        /// <returns>True if updated successfully</returns>
        public override async Task<bool> Update(Guid Id, Recipe recipe)
        {
            try
            {
                if (_parentStore.Current == null)
                    return false;

                Recipe updatedRecipe = await _dataService.Update(Id, recipe);
                _parentStore.Current.RemoveRecipe(Id);
                _parentStore.Current.AddRecipe(updatedRecipe);

                _items.Add(recipe);
                Recipe item = _items.FirstOrDefault(r => r.Id == Id);
                if (item != null)
                    _items.Remove(item);

                OnItemUpdated(updatedRecipe);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public override async Task<bool> Move(Recipe recipe, Guid newParentId) 
        {
            RecipeCategory? oldParent = _parentStore.Items.FirstOrDefault(c => c.Id == recipe.ParentId);
            recipe.ParentId = newParentId;
            if (await _childDataService.Move(recipe.Id, newParentId))
            {
                oldParent?.RemoveRecipe(recipe.Id);
                RecipeCategory? newParent = _parentStore.Items.FirstOrDefault(c => c.Id == recipe.ParentId);
                newParent?.AddRecipe(recipe);

                _items.Add(recipe);
                return true;
            }
            return false;
        }

        public override async Task<Recipe?> Duplicate(Guid Id)
        {
            Recipe recipe = await Get(Id);
            RecipeCategory category = await _parentStore.Get(recipe.ParentId);
            category.AddRecipe(recipe);
            await _parentStore.Update(recipe.ParentId, category);
            return await base.Duplicate(Id);
        }
    }
}
