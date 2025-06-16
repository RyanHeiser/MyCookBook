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
        public Recipe Recipe { get; }

        public string Name => Recipe.Name;
        public string Minutes => Recipe.Minutes != 0 ? Recipe.Minutes.ToString() : String.Empty;
        public string Servings => Recipe.Servings != 0 ? Recipe.Servings.ToString() : String.Empty;
        public IEnumerable<string> Ingredients => Recipe.Ingredients;
        public IEnumerable<string> Directions => Recipe.Directions;

        public RecipeViewModel(Recipe recipe)
        {
            Recipe = recipe;
        }
    }
}
