using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyCookBook.WPF.Commands
{
    public class UpdateRecipeCommand : AsyncCommandBase
    {
        private readonly CreateRecipeViewModel _createRecipeViewModel;
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;

        public UpdateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, RecipeBookStore recipeBookStore, RecipeStore recipeStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _createRecipeViewModel.IsSubmitting = true;

            Recipe? recipe = _createRecipeViewModel.Recipe;

            Recipe updatedRecipe = new Recipe(
                _createRecipeViewModel.Name ?? "New Recipe",
                _createRecipeViewModel.Minutes,
                _createRecipeViewModel.Servings,
                _createRecipeViewModel.RawThumbnailData,
                new List<string>(_createRecipeViewModel.Ingredients.Where(i => !String.IsNullOrEmpty(i.Text)).Select(i => i.Text)), // Convert non-empty Ingredient StringViewModels to Strings
                new List<string>(_createRecipeViewModel.Directions.Where(d => !String.IsNullOrEmpty(d.Text)).Select(d => d.Text)),  // Convert non-empty Direction StringViewModels to Strings
                recipe.CategoryId);

            RecipeImage image = new RecipeImage(recipe.RecipeId, _createRecipeViewModel.RawImageData);

            _recipeStore.CurrentRecipe = updatedRecipe;

            try
            {
                await _recipeBookStore.UpdateRecipe(recipe.RecipeId, updatedRecipe, _recipeStore.CurrentCategory);
                await _recipeBookStore.UpdateImage(recipe.RecipeId, image);
                MessageBox.Show("Updated recipe", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }

            _createRecipeViewModel.IsSubmitting = false;
        }
    }
}
