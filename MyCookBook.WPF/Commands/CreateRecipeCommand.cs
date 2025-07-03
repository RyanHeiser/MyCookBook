using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
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
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;

        public CreateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, RecipeBookStore recipeBookStore, RecipeStore recipeStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _createRecipeViewModel.PropertyChanged += OnViewModelPropertyChanged;
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
                _createRecipeViewModel.RawThumbnailData,
                new List<string>(_createRecipeViewModel.Ingredients.Where(i => !String.IsNullOrEmpty(i.Text)).Select(i => i.Text)), // Convert non-empty Ingredient StringViewModels to Strings
                new List<string>(_createRecipeViewModel.Directions.Where(d => !String.IsNullOrEmpty(d.Text)).Select(d => d.Text)), // Convert non-empty Direction StringViewModels to Strings
                _recipeStore.CurrentCategory.CategoryId);

            RecipeImage image = new RecipeImage(recipe.RecipeId, _createRecipeViewModel.RawImageData);

            _recipeStore.CurrentRecipe = recipe;

            try
            {
                await _recipeBookStore.CreateRecipe(recipe, _createRecipeViewModel.Category ?? new RecipeCategory("New Category", new List<Recipe>()));
                await _recipeBookStore.CreateImage(image);
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
