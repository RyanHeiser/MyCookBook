using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class SetCurrentRecipeCommand : CommandBase
    {
        private readonly RecipeStore _recipeStore;

        public SetCurrentRecipeCommand(RecipeStore recipeStore)
        {
            _recipeStore = recipeStore;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                Recipe? recipe = parameter as Recipe;
                _recipeStore.CurrentRecipe = recipe;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
