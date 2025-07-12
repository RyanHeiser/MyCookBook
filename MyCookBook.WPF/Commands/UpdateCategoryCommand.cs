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
    public class UpdateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _createCategoryViewModel;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public UpdateCategoryCommand(CreateCategoryViewModel createCategoryViewModel, RecipeStoreBase<RecipeBook> recipeBookStore, 
            RecipeStoreBase<RecipeCategory> categoryStore)
        {
            _createCategoryViewModel = createCategoryViewModel;
            _recipeBookStore = recipeBookStore;
            _categoryStore = categoryStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeCategory updatedCategory = new RecipeCategory(_createCategoryViewModel.Name, _categoryStore.Current.RecipeCount);

            try
            {
                await _categoryStore.Update(_categoryStore.Current.Id, updatedCategory);
                MessageBox.Show("Updated category", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }
        }
    }
}
