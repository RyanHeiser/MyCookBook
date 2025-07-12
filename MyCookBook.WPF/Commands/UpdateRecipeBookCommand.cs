using MyCookBook.Domain.Models;
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
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;

        public UpdateRecipeBookCommand(CreateRecipeBookViewModel createRecipeBookViewModel, RecipeStoreBase<RecipeBook> recipeBookStore)
        {
            _createRecipeBookViewModel = createRecipeBookViewModel;
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeBook current = _recipeBookStore.Current;
            RecipeBook updatedBook = new RecipeBook(_createRecipeBookViewModel.Name);

            try
            {
                await _recipeBookStore.Update(_recipeBookStore.Current.Id, updatedBook);
                MessageBox.Show("Updated category", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }
        }
    }
}