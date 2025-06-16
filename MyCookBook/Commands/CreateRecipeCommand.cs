using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using MyCookBook.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCookBook.Commands
{
    public class CreateRecipeCommand : AsyncCommandBase
    {
        private readonly CreateRecipeViewModel _createRecipeViewModel;
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;
        private readonly NavigationService<RecipeDisplayViewModel> _recipeDisplayNavigationService;

        public CreateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, RecipeBookStore recipeBookStore, RecipeStore recipeStore, NavigationService<RecipeDisplayViewModel> recipeDisplayNavigationService)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _recipeDisplayNavigationService = recipeDisplayNavigationService;
            _createRecipeViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _createRecipeViewModel.IsSubmitting = true;

            Recipe recipe = new Recipe(
                _createRecipeViewModel.Name,
                _createRecipeViewModel.Minutes,
                _createRecipeViewModel.Servings,
                new List<string>(_createRecipeViewModel.Ingredients.Select(i => i.Text)),
                new List<string>(_createRecipeViewModel.Directions.Select(d => d.Text)));

            _recipeStore.CurrentRecipe = recipe;

            try
            {
                await _recipeBookStore.CreateRecipe(recipe, _createRecipeViewModel.Category);
                MessageBox.Show("Created recipe", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _recipeDisplayNavigationService.Navigate();
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
