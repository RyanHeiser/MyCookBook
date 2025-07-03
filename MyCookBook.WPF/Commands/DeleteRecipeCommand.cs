using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    class DeleteRecipeCommand : AsyncCommandBase
    {
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeCategory _category;

        public DeleteRecipeCommand(RecipeBookStore recipeBookStore, RecipeCategory category)
        {
            _recipeBookStore = recipeBookStore;
            _category = category;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Recipe? recipe = parameter as Recipe;
                await _recipeBookStore.DeleteRecipe(recipe.RecipeId, _category);
            }
            catch (Exception)
            {
                throw new InvalidCastException();
            }
        }
    }
}
