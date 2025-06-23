using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class LoadRecipeCategoriesCommand : AsyncCommandBase
    {
        private readonly CategoryListingViewModel _viewModel;
        private readonly RecipeBookStore _recipeBookStore;

        public LoadRecipeCategoriesCommand(CategoryListingViewModel viewModel, RecipeBookStore recipeBookStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.IsLoading = true;

            try
            {
                await _recipeBookStore.LoadCategories();
                _viewModel.UpdateCategories(_recipeBookStore.RecipeCategories);
            }
            catch (Exception)
            {
               
            }

            _viewModel.IsLoading = false;
        }
    }
}
