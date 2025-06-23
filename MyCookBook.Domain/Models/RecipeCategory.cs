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

            //// !!!!! TODO move to dependency injection (factory?) !!!!
            //_recipeDataService = new RecipeDataService(Id, new CategoryDataService(new MyCookBookDbContextFactory()));
            //_recipeDTOConverter = new RecipeDTOConverter();
        }

        public RecipeCategory(Guid id, string name, List<Recipe> recipes)
        {
            Id = id;
            Name = name;
            _recipes = recipes;

            //// !!!! TODO move to dependency injection (factory?) !!!!
            //_recipeDataService = new RecipeDataService(Id, new CategoryDataService(new MyCookBookDbContextFactory()));
            //_recipeDTOConverter = new RecipeDTOConverter();
        }

        /// <summary>
        /// Gets all the categories in the ReservationBook.
        /// </summary>
        /// <returns>An IEnumerable comtaining the categories</returns>
        //public async Task<IEnumerable<Recipe>> GetAllRecipes()
        //{
        //    IEnumerable<RecipeDTO> dtos = await _recipeDataService.GetAll();

        //    return new List<Recipe>(dtos.Select(d => _recipeDTOConverter.ConvertFromDTO(d)));
        //}

        /// <summary>
        /// Adds a recipe in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public void AddRecipe(Recipe recipe)
        {   
            _recipes.Add(recipe);

            //RecipeDTO dto = _recipeDTOConverter.ConvertToDTO(recipe);
            //await _recipeDataService.Create(dto);
        }

        /// <summary>
        /// Adds a range of recipes in alphabetical order
        /// </summary>
        /// <param name="recipes">The recipes to add</param>
        public void AddRecipes(IEnumerable<Recipe> recipes)
        {
            foreach (Recipe recipe in recipes)
            {
                AddRecipe(recipe);
            }
        }

        /// <summary>
        /// Updates a recipe.
        /// </summary>
        /// <param name="Id">The Id of the recipe to update.</param>
        /// <param name="recipe">The new recipe replacing the old value</param>
        /// <returns>True if successful</returns>
        //public async Task<bool> UpdateRecipe(Guid Id, Recipe recipe)
        //{
        //    RecipeDTO dto = _recipeDTOConverter.ConvertToDTO(recipe);
        //    return _recipeDTOConverter.UpdateFromDTO(await _recipeDataService.Update(Id, dto), recipe);
        //}

        /// <summary>
        /// Removes a recipe by id.
        /// </summary>
        /// <param name="id">The id of the recipe to remove.</param>
        //public async Task<bool> RemoveRecipe(Recipe target)
        //{
        //    return await _recipeDataService.Delete(Id);
        //}

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
