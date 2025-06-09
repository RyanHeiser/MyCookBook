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
        public Recipe _recipe { get; }

        public string Name => _recipe.Name;
        public string Minutes => _recipe.Minutes != 0 ? _recipe.Minutes.ToString() : String.Empty;
        public string Servings => _recipe.Servings != 0 ? _recipe.Servings.ToString() : String.Empty;
        public IEnumerable<string> Ingredients => _recipe.Ingredients;
        public IEnumerable<string> Directions => _recipe.Directions;

        public RecipeViewModel(Recipe recipe)
        {
            _recipe = recipe;
        }
    }
}
