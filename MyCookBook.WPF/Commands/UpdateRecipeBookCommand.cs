﻿using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCookBook.WPF.Commands
{
    public class UpdateRecipeBookCommand : AsyncCommandBase
    {
        private readonly CreateRecipeBookViewModel _createRecipeBookViewModel;
        private readonly INavigationService _navigationService;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;

        public UpdateRecipeBookCommand(CreateRecipeBookViewModel createRecipeBookViewModel, INavigationService navigationService, RecipeStoreBase<RecipeBook> recipeBookStore)
        {
            _createRecipeBookViewModel = createRecipeBookViewModel;
            _recipeBookStore = recipeBookStore;
            _navigationService = navigationService;
        }

        /// <summary>
        /// Updates the current RecipeBook from CreateRecipeBookView.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (_createRecipeBookViewModel.Name.Length > 50)
            {
                _createRecipeBookViewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            RecipeBook current = _recipeBookStore.Current;
            RecipeBook updatedBook = new RecipeBook(_createRecipeBookViewModel.Name);

            try
            {
                await _recipeBookStore.Update(_recipeBookStore.Current.Id, updatedBook);

                _navigationService.Navigate();
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }
        }
    }
}