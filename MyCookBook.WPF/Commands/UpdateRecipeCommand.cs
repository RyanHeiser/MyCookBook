﻿using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
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
        private readonly INavigationService _navigationService;
        private readonly RecipeStoreBase<RecipeImage> _imageStore;
        private readonly RecipeStoreBase<Recipe> _recipeStore;

        public UpdateRecipeCommand(CreateRecipeViewModel createRecipeViewModel, INavigationService navigationService, RecipeStoreBase<Recipe> recipeStore, RecipeStoreBase<RecipeImage> imageStore)
        {
            _createRecipeViewModel = createRecipeViewModel;
            _imageStore = imageStore;
            _recipeStore = recipeStore;
            _navigationService = navigationService;
        }

        /// <summary>
        /// Updates the current Recipe from CreateRecipeView.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (_createRecipeViewModel?.Name?.Length > 50)
            {
                _createRecipeViewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            _createRecipeViewModel.IsSubmitting = true;

            Recipe? recipe = _createRecipeViewModel.Recipe;

            Recipe updatedRecipe = new Recipe(
                _createRecipeViewModel.Name ?? "New Recipe",
                _createRecipeViewModel.Hours,
                _createRecipeViewModel.Minutes,
                _createRecipeViewModel.Servings,
                _createRecipeViewModel.Description,
                _createRecipeViewModel.RawThumbnailData,
                new List<string>(_createRecipeViewModel.Ingredients.Where(i => !String.IsNullOrEmpty(i.Text)).Select(i => i.Text)), // Convert non-empty Ingredient StringViewModels to Strings
                new List<string>(_createRecipeViewModel.Directions.Where(d => !String.IsNullOrEmpty(d.Text)).Select(d => d.Text)));  // Convert non-empty Direction StringViewModels to Strings

            RecipeImage image = new RecipeImage(_createRecipeViewModel.RawImageData);

            _recipeStore.Current = updatedRecipe;

            try
            {
                await _recipeStore.Update(recipe.Id, updatedRecipe);

                if (_imageStore.Items != null)
                    await _imageStore.Update(_imageStore.Items.First().Id, image);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }

            _createRecipeViewModel.IsSubmitting = false;

            _navigationService.Navigate();
        }
    }
}
