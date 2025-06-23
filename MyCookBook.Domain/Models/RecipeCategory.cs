using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeCategory
    {
        //private readonly RecipeDataService _recipeDataService;
        //private readonly RecipeDTOConverter _recipeDTOConverter;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Recipe> _recipes;
        public IEnumerable<Recipe> Recipes => _recipes;

        public RecipeCategory(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            _recipes = new List<Recipe>();
        }

        public RecipeCategory(string name, List<Recipe> recipes)
        {
            Id = Guid.NewGuid();
            Name = name;
            _recipes = recipes;
        }

        public RecipeCategory(Guid id, string name, List<Recipe> recipes)
        {
            Id = id;
            Name = name;
            _recipes = recipes;
        }

        /// <summary>
        /// Adds a recipe in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public void AddRecipe(Recipe recipe)
        {   
            _recipes.Add(recipe);
        }


        /// <summary>
        /// Updates a recipe.
        /// </summary>
        /// <param name="Id">The Id of the recipe to update.</param>
        /// <param name="recipe">The new recipe replacing the old value</param>
        /// <returns>True if successful</returns>
        public bool UpdateRecipe(Guid Id, Recipe updatedRecipe)
        {
            Recipe? existing = Recipes.FirstOrDefault(r => r.Id == Id);

            if (existing != null)
            {
                _recipes.Remove(existing);
                updatedRecipe.Id = Id;
                _recipes.Add(updatedRecipe);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a recipe by id.
        /// </summary>
        /// <param name="id">The id of the recipe to remove.</param>
        public bool RemoveRecipe(Recipe target)
        {
            Recipe? existing = Recipes.FirstOrDefault(r => r.Id == Id);

            if (existing != null)
            {
                _recipes.Remove(existing);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears the recipes.
        /// TODO implement with database
        /// </summary>
        public void ClearRecipes()
        {
            _recipes.Clear();
        }
    }
}
