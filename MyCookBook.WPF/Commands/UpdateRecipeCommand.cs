using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
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
        private readonly RecipeImageStore _imageStore;
        private readonly RecipeStore _recipeStore;

        public UpdateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, RecipeStore recipeStore, RecipeImageStore imageStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _imageStore = imageStore;
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
                new List<string>(_createRecipeViewModel.Directions.Where(d => !String.IsNullOrEmpty(d.Text)).Select(d => d.Text)));  // Convert non-empty Direction StringViewModels to Strings

            RecipeImage image = new RecipeImage(_createRecipeViewModel.RawImageData);

            //if (_imageStore.Items.Any())
            //{
            //    image.Id = _imageStore.Items.First().Id;
            //}

            //updatedRecipe.Image = image;

            _recipeStore.Current = updatedRecipe;

            try
            {
                await _recipeStore.Update(recipe.Id, updatedRecipe);

                if (_imageStore.Items != null)
                    await _imageStore.Update(_imageStore.Items.First().Id, image);

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
