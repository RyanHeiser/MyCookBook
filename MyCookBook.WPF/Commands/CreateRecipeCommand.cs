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
        private readonly INavigationService _navigationService;
        private readonly RecipeStoreBase<RecipeImage> _imageStore;
        private readonly RecipeStoreBase<Recipe> _recipeStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public CreateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, INavigationService navigationService, RecipeStoreBase<RecipeCategory> categoryStore, RecipeStoreBase<Recipe> recipeStore, RecipeStoreBase<RecipeImage> imageStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _navigationService = navigationService;
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
            if (_createRecipeViewModel?.Name?.Length > 50)
            {
                _createRecipeViewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            _createRecipeViewModel.IsSubmitting = true;

            Recipe recipe = new Recipe(
                _createRecipeViewModel.Name ?? "New Recipe",
                _createRecipeViewModel.Hours,
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
            } 
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }

            _createRecipeViewModel.IsSubmitting = false;

            _navigationService.Navigate();
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
