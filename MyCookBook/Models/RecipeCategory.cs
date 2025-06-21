using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Models
{
    public class RecipeCategory
    {
        public Guid Id { get; }
        public string Name { get; set; }

        public List<Recipe> _recipes;
        public IEnumerable<Recipe> Recipes => _recipes;

        public RecipeCategory(string name, List<Recipe> recipes)
        {
            Id = Guid.NewGuid();
            Name = name;
            _recipes = recipes;
        }

        /// <summary>
        /// Gets all the categories in the ReservationBook.
        /// TODO: Implement database to pull from
        /// </summary>
        /// <returns>An IEnumerable comtaining the categories</returns>
        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _recipes;
        }

        /// <summary>
        /// Adds a recipe in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public int AddRecipe(Recipe recipe)
        {   
            // Searches for position to add new recipe
            for (int i = 0; i < _recipes.Count; i++)
            {
                if (string.Compare(_recipes[i].Name, recipe.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    _recipes.Insert(i, recipe);
                    return i;
                }
            }

            // adds recipe to end of list if it is last in alphabetical order
            _recipes.Add(recipe);
            return _recipes.Count - 1;
        }

        /// <summary>
        /// Removes a recipe by id.
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
