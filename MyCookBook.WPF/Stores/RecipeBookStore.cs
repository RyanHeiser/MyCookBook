using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class RecipeBookStore
    {
        private readonly RecipeBook _recipeBook;
        private readonly RecipeStore _recipeStore;
        private readonly CategoryDataService _dataService;
        private List<RecipeCategory> _categories;
        private List<Recipe> _recipes;
        private Lazy<Task> _categoryLazy;

        public IEnumerable<RecipeCategory> RecipeCategories => _categories;

        public IEnumerable<Recipe> Recipes => _recipes;

        public event Action<RecipeCategory>? CategoryCreated;

        public event Action<Recipe, RecipeCategory>? RecipeCreated;

        public RecipeBookStore(RecipeBook recipeBook, RecipeStore recipeStore, CategoryDataService dataService)
        {
            _recipeBook = recipeBook;
            _recipeStore = recipeStore;
            _dataService = dataService;
            _categories = new List<RecipeCategory>();
            _recipes = new List<Recipe>();
            _categoryLazy = new Lazy<Task>(InitializeCategories);
        }

        public async Task LoadCategories()
        {
            try
            {
                await _categoryLazy.Value;
            }
            catch
            {
                _categoryLazy = new Lazy<Task>(InitializeCategories);
                throw;
            }
        }

        public async Task LoadRecipes()
        {
            _recipes = _recipeStore.CurrentCategory?.Recipes.ToList() ?? new List<Recipe>();
        }

        public async Task CreateCategory(RecipeCategory category)
        {
            await _dataService.Create(category);
            _categories.Add(category);

            OnCategoryCreated(category);
        }

        /// <summary>
        /// Add a recipe
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <param name="category">The category of the recipe</param>
        public async Task CreateRecipe(Recipe recipe, RecipeCategory category)
        {
            if (!_categories.Contains(category))
            {
                await CreateCategory(category);
            }
            await _dataService.CreateRecipe(recipe);
            category.AddRecipe(recipe);
            //_recipes.Add(recipe);

            OnRecipeCreated(recipe, category);
        }

        public async Task<bool> DeleteCategory(Guid Id)
        {
            return await _dataService.Delete(Id);
        }

        public async Task<bool> DeleteRecipe(Guid Id, RecipeCategory category)
        {
            if (await _dataService.DeleteRecipe(Id))
            {
                category.RemoveRecipe(Id);
                return true;
            }
            return false;
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            CategoryCreated?.Invoke(category);
        }

        private void OnRecipeCreated(Recipe recipe, RecipeCategory category)
        {
            RecipeCreated?.Invoke(recipe, category);
        }

        private async Task InitializeCategories()
        {
            IEnumerable<RecipeCategory> recipeCategories = await _dataService.GetAll();

            _categories.Clear();
            _categories.AddRange(recipeCategories);
        }
    }
}
