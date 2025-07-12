using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCookBook.WPF.Commands
{
    public class CreateRecipeCommand : AsyncCommandBase
    {
        private readonly CreateRecipeViewModel _createRecipeViewModel;
        private readonly RecipeStoreBase<RecipeImage> _imageStore;
        private readonly RecipeStoreBase<Recipe> _recipeStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public CreateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, RecipeStoreBase<RecipeCategory> categoryStore, RecipeStoreBase<Recipe> recipeStore, RecipeStoreBase<RecipeImage> imageStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _recipeStore = recipeStore;
            _categoryStore = categoryStore;
            _createRecipeViewModel.PropertyChanged += OnViewModelPropertyChanged;
            _imageStore = imageStore;
        }

        public override bool CanExecute(object? parameter)
        {
            return _createRecipeViewModel.CanCreateRecipe && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _createRecipeViewModel.IsSubmitting = true;

            Recipe recipe = new Recipe(
                _createRecipeViewModel.Name ?? "New Recipe",
                _createRecipeViewModel.Minutes,
                _createRecipeViewModel.Servings,
                _createRecipeViewModel.Description,
                _createRecipeViewModel.RawThumbnailData,
                new List<string>(_createRecipeViewModel.Ingredients.Where(i => !String.IsNullOrEmpty(i.Text)).Select(i => i.Text)), // Convert non-empty Ingredient StringViewModels to Strings
                new List<string>(_createRecipeViewModel.Directions.Where(d => !String.IsNullOrEmpty(d.Text)).Select(d => d.Text)), // Convert non-empty Direction StringViewModels to Strings
                _categoryStore.Current.Id);

            RecipeImage image = new RecipeImage(_createRecipeViewModel.RawImageData, recipe.Id);

            recipe.Image = image;

            _recipeStore.Current = recipe;

            try
            {
                await _recipeStore.Create(recipe);
                MessageBox.Show("Created recipe", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }

            _createRecipeViewModel.IsSubmitting = false;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CreateRecipeViewModel.CanCreateRecipe))
            {
                OnCanExecuteChanged();
            }
        }
    }
}
