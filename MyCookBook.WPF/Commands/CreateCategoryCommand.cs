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
    public class CreateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _viewModel;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public CreateCategoryCommand(CreateCategoryViewModel viewModel, RecipeStoreBase<RecipeBook> recipeBookStore, RecipeStoreBase<RecipeCategory> categoryStore)
        {
            _viewModel = viewModel;
            _recipeBookStore = recipeBookStore;
            _categoryStore = categoryStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeCategory category = new RecipeCategory(_viewModel.Name, _recipeBookStore.Current.Id);

            await _categoryStore.Create(category);
        }
    }
}
