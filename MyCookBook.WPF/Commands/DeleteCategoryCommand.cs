using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    class DeleteCategoryCommand : AsyncCommandBase
    {
        private readonly RecipeBookStore _recipeBookStore;

        public DeleteCategoryCommand(RecipeBookStore recipeBookStore)
        {
            _recipeBookStore = recipeBookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                RecipeCategory? category = parameter as RecipeCategory;
                await _recipeBookStore.DeleteCategory(category.CategoryId);
            }
            catch (Exception)
            {
                throw new InvalidCastException();
            }
        }
    }
}
