using MyCookBook.Models;
using MyCookBook.Stores;
using MyCookBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Commands
{
    public class CreateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _viewModel;
        private readonly RecipeBookStore _recipeBookStore;

        public CreateCategoryCommand(CreateCategoryViewModel viewModel, RecipeBookStore recipeBookStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeCategory category = new RecipeCategory(_viewModel.Name, new List<Recipe>());

            await _recipeBookStore.CreateRecipeCategory(category);
        }
    }
}
