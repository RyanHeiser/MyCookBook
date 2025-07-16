using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeCategory : ChildDomainObject
    {
        public string Name { get; set; }

        [JsonInclude]
        public int RecipeCount { get; private set; }
        [JsonInclude]
        public List<Recipe> _recipes;
        [JsonIgnore]
        public IEnumerable<Recipe> Recipes => _recipes;

        public event Action<int>? RecipeCountChanged;

        [JsonConstructor]
        public RecipeCategory(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            RecipeCount = 0;
            _recipes = new List<Recipe>();
        }

        public RecipeCategory(string name, Guid bookId)
        {
            Id = Guid.NewGuid();
            Name = name;
            RecipeCount = 0;
            _recipes = new List<Recipe>();
            ParentId = bookId;
        }

        public RecipeCategory(string name, int recipeCount)
        {
            Id = Guid.NewGuid();
            Name = name;
            RecipeCount = recipeCount;
            _recipes = new List<Recipe>();
        }

        /// <summary>
        /// Adds a recipe in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public void AddRecipe(Recipe recipe)
        {   
            //_recipes.Add(recipe);
            RecipeCount++;
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
        public bool RemoveRecipe(Guid Id)
        {
            //Recipe? existing = Recipes.FirstOrDefault(r => r.Id == Id);

            //if (existing != null)
            //{
            //    _recipes.Remove(existing);
            //    RecipeCount--;
            //    return true;
            //}
            //return false;
            RecipeCount--;
            return true;
        }

        /// <summary>
        /// Clears the recipes.
        /// TODO implement with database
        /// </summary>
        public void ClearRecipes()
        {
            OnRecipeCountChanged(-RecipeCount);
            _recipes.Clear();
            RecipeCount = 0;
        }

        private void OnRecipeCountChanged(int change)
        {
            RecipeCountChanged?.Invoke(change);
        }
    }
}
