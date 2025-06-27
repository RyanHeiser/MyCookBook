using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
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
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;

        public UpdateCategoryCommand(CreateCategoryViewModel createCategoryViewModel, RecipeBookStore recipeBookStore, RecipeStore recipeStore)
        {
            _createCategoryViewModel = createCategoryViewModel;
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            RecipeCategory updatedCategory = new RecipeCategory(_createCategoryViewModel.Name);

            try
            {
                await _recipeBookStore.UpdateCategory(_createCategoryViewModel.Category.CategoryId, updatedCategory);
                MessageBox.Show("Updated category", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Null Reference");
            }
        }
    }
}
