using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CreateRecipeBookCommand : AsyncCommandBase
    {
        private readonly CreateRecipeBookViewModel _viewModel;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;

        public CreateRecipeBookCommand(CreateRecipeBookViewModel viewModel, RecipeStoreBase<RecipeBook> recipeBookStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_viewModel.Name.Length > 50)
            {
                _viewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            RecipeBook book = new RecipeBook(_viewModel.Name);

            bool created = await _recipeBookStore.Create(book);

            if (!created)
            {
                _viewModel.ErrorMessage = "Could not create this recipe book.";
            }
        }
    }
}