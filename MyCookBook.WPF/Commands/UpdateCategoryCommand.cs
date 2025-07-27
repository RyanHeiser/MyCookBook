using MyCookBook.Domain.Models;
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
    public class UpdateCategoryCommand : AsyncCommandBase
    {
        private readonly CreateCategoryViewModel _createCategoryViewModel;
        private readonly INavigationService _navigationService;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

        public UpdateCategoryCommand(CreateCategoryViewModel createCategoryViewModel, INavigationService navigationService, RecipeStoreBase<RecipeBook> recipeBookStore,
            RecipeStoreBase<RecipeCategory> categoryStore)
        {
            _createCategoryViewModel = createCategoryViewModel;
            _recipeBookStore = recipeBookStore;
            _categoryStore = categoryStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_createCategoryViewModel.Name.Length > 50)
            {
                _createCategoryViewModel.ErrorMessage = "Name must not be longer than 50 characters";
                return;
            }

            RecipeCategory updatedCategory = new RecipeCategory(_createCategoryViewModel.Name, _categoryStore.Current.RecipeCount);

            try
            {
                await _categoryStore.Update(_categoryStore.Current.Id, updatedCategory);
                MessageBox.Show("Updated category", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _navigationService.Navigate();
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }
        }
    }
}
