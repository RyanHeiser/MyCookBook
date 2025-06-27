using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class SetCurrentCategoryCommand : CommandBase
    {
        private readonly RecipeStore _recipeStore;

        public SetCurrentCategoryCommand(RecipeStore recipeStore)
        {
            _recipeStore = recipeStore;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                RecipeCategory? category = parameter as RecipeCategory;
                _recipeStore.CurrentCategory = category;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
