using MyCookBook.Domain.Models;
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
        private List<RecipeCategory> _recipeCategories;
        private List<Recipe> _recipes;
        private Lazy<Task> _categoryLazy;
        private Lazy<Task> _recipeLazy;

        public IEnumerable<RecipeCategory> RecipeCategories => _recipeCategories;

        public IEnumerable<Recipe> Recipes => _recipes;

        public event Action<RecipeCategory>? CategoryCreated;

        public event Action<Recipe, RecipeCategory>? RecipeCreated;

        public RecipeBookStore(RecipeBook recipeBook, RecipeStore recipeStore)
        {
            _recipeBook = recipeBook;
            _recipeStore = recipeStore;
            _recipeCategories = new List<RecipeCategory>();
            _recipes = new List<Recipe>();
            _categoryLazy = new Lazy<Task>(InitializeCategories);
            _recipeLazy = new Lazy<Task>(InitializeRecipes);

            _recipeStore.CategoryChanged += OnCurrentCategoryChanged;
        }

        private void OnCurrentCategoryChanged(RecipeCategory category)
        {
            _recipeLazy = new Lazy<Task>(InitializeRecipes);
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
            try
            {
                await _recipeLazy.Value;
            }
            catch
            {
                _recipeLazy = new Lazy<Task>(InitializeRecipes);
                throw;
            }
        }

        public async Task CreateRecipeCategory(RecipeCategory category)
        {
            await _recipeBook.AddRecipeCategory(category);
            _recipeCategories.Add(category);

            OnCategoryCreated(category);
        }

        /// <summary>
        /// Add a recipe
        /// </summary>
        /// <param name="recipe">The recipe to add</param>
        /// <param name="category">The category of the recipe</param>
        public async Task CreateRecipe(Recipe recipe, RecipeCategory category)
        {
            if (!_recipeCategories.Contains(category))
            {
                await CreateRecipeCategory(category);
            }
            category.AddRecipe(recipe);
            _recipes.Add(recipe);
            await _recipeBook.UpdateRecipeCategory(category.Id, category);

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

        private async Task InitializeCategories()
        {
            IEnumerable<RecipeCategory> recipeCategories = await _recipeBook.GetAllCategories();

            _recipeCategories.Clear();
            _recipeCategories.AddRange(recipeCategories);
        }

        private async Task InitializeRecipes()
        {
            RecipeCategory category = await _recipeBook.GetCategory(_recipeStore.CurrentCategory.Id);
            IEnumerable<Recipe> recipes = category.Recipes;

            _recipes.Clear();
            _recipes.AddRange(recipes);
        }
    }
}
