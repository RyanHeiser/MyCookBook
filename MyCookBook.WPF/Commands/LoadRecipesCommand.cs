using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class LoadRecipesCommand : AsyncCommandBase
    {
        private readonly RecipeListingViewModel _viewModel;
        private readonly RecipeBookStore _recipeBookStore;

        public LoadRecipesCommand(RecipeListingViewModel viewModel, RecipeBookStore recipeBookStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.IsLoading = true;

            try
            {
                await _recipeBookStore.LoadRecipes();
                _viewModel.UpdateRecipes(_recipeBookStore.Recipes);
            }
            catch (Exception)
            {

            }

            _viewModel.IsLoading = false;
        }
    }
}
