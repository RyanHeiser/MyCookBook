using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels;
using MyCookBook.WPF.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CreateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _viewModel;
        private readonly INavigationService _navigationService;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public CreateCategoryCommand(CreateCategoryViewModel viewModel, INavigationService navigationService, RecipeStoreBase<RecipeBook> recipeBookStore, RecipeStoreBase<RecipeCategory> categoryStore)
        {
            _viewModel = viewModel;
            _navigationService = navigationService;
            _recipeBookStore = recipeBookStore;
            _categoryStore = categoryStore;
        }

        /// <summary>
        /// Creates a new Category.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override async Task ExecuteAsync(object? parameter)
        {
            if (_viewModel.Name.Length > 50)
            {
                _viewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            RecipeCategory category = new RecipeCategory(_viewModel.Name, _recipeBookStore.Current.Id);

            await _categoryStore.Create(category);

            _navigationService.Navigate();
        }
    }
}
