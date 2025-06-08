using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Models
{
    class RecipeCategory
    {
        public string Name { get; set; }

        private List<Recipe> _recipes;
        public IEnumerable<Recipe> Recipes { get { return _recipes; } }

        public RecipeCategory(string name, List<Recipe> recipes)
        {
            Name = name;
            _recipes = recipes;
        }

        /// <summary>
        /// Adds a recipe to _recipes in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public void AddRecipe(Recipe recipe)
        {
            _recipes.Add(recipe);
            for (int i = 0; i < _recipes.Count; i++)
            {
                if (string.Compare(_recipes[i].Name, recipe.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    _recipes.Insert(i, recipe);
                }
            }
        }

        /// <summary>
        /// Removes a recipe from _recipes by id.
        /// </summary>
        /// <param name="id">The id of the recipe to remove.</param>
        public void RemoveRecipe(Guid id)
        {
            foreach (Recipe recipe in _recipes)
            {
                if (recipe.Id == id)
                {
                    _recipes.Remove(recipe);
                }
            }
        }

        /// <summary>
        /// Clears the recipes.
        /// </summary>
        public void ClearRecipes()
        {
            _recipes.Clear();
        }
    }
}
