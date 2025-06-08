using MyCookBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    public class RecipeViewModel
    {
        private readonly Recipe _recipe;

        public string Name => _recipe.Name;
        public string Minutes => _recipe.Minutes.ToString();
        public string Servings => _recipe.Servings.ToString();
        public IEnumerable<string> Ingredients => _recipe.Ingredients;
        public IEnumerable<string> Directions => _recipe.Directions;

        public RecipeViewModel(Recipe recipe)
        {
            _recipe = recipe;
        }
    }
}
