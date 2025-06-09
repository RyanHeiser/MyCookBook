using MyCookBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Stores
{
    public class RecipeBookStore
    {
        private readonly RecipeBook _recipeBook;
        private List<RecipeCategory> _recipeCategories;
        private Lazy<Task> _initLazy;

        public IEnumerable<RecipeCategory> RecipeCategories => _recipeCategories;

        public event Action<RecipeCategory> CategoryCreated;

        public event Action<Recipe, RecipeCategory> RecipeCreated;

        public RecipeBookStore(RecipeBook recipeBook)
        {
            _recipeBook = recipeBook;
            _recipeCategories = new List<RecipeCategory>(recipeBook.Categories);
            //_initLazy = new Lazy<Task>(Initialize);
        }

        public void CreateRecipeCategory(RecipeCategory category)
        {
            int index = _recipeBook.AddRecipeCategory(category);
            _recipeCategories.Insert(index, category);

            OnCategoryCreated(category);
        }

        public void CreateRecipe(Recipe recipe, RecipeCategory category)
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
                CreateRecipeCategory(category);
                category.AddRecipe(recipe);
            }
            else
            {
                _recipeCategories.ElementAt(categoryIndex).AddRecipe(recipe);
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

        //private async Task Initialize()
        //{
        //    IEnumerable<RecipeCategory> recipeCategories = await _recipeBook.GetAllCategories();

        //    _recipeCategories.Clear();
        //    _recipeCategories.AddRange(recipeCategories);
        //}
    }
}
