using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class RecipeBookStore
    {
        private readonly RecipeBook _recipeBook;
        private List<RecipeCategory> _recipeCategories;
        private Lazy<Task> _initLazy;

        public IEnumerable<RecipeCategory> RecipeCategories => _recipeCategories;

        public event Action<RecipeCategory>? CategoryCreated;

        public event Action<Recipe, RecipeCategory>? RecipeCreated;

        public RecipeBookStore(RecipeBook recipeBook)
        {
            _recipeBook = recipeBook;
            _recipeCategories = new List<RecipeCategory>(recipeBook.Categories);
            _initLazy = new Lazy<Task>(Initialize);
        }

        public async Task Load()
        {
            try
            {
                await _initLazy.Value;
            }
            catch
            {
                _initLazy = new Lazy<Task>(Initialize);
                throw;
            }
        }

        public async Task CreateRecipeCategory(RecipeCategory category)
        {
            int index = _recipeBook.AddRecipeCategory(category);
            _recipeCategories.Insert(index, category);

            OnCategoryCreated(category);
        }

        /// <summary>
        /// Add a recipe
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <param name="category">The category of the recipe</param>
        public async Task CreateRecipe(Recipe recipe, RecipeCategory category)
        {
            int categoryIndex = -1;
            for (int i = 0; i < _recipeBook.Categories.Count(); i++)
            {
                RecipeCategory c = _recipeBook.Categories.ElementAt(i);
                if (c == category)
                {
                    c.AddRecipe(recipe);
                    categoryIndex = i;
                    break;
                }
            }
            if (categoryIndex == -1)
            {
                await CreateRecipeCategory(category);
                await CreateRecipe(recipe, category);
            }

            OnRecipeCreated(recipe, category);
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            CategoryCreated?.Invoke(category);
        }

        private void OnRecipeCreated(Recipe recipe, RecipeCategory category)
        {
            RecipeCreated?.Invoke(recipe, category);
        }

        private async Task Initialize()
        {
            IEnumerable<RecipeCategory> recipeCategories = _recipeBook.GetAllCategories();

            _recipeCategories.Clear();
            _recipeCategories.AddRange(recipeCategories);
        }
    }
}
