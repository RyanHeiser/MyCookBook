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
        private readonly IDataService<RecipeCategory> _categoryDataService;
        private readonly IDataService<Recipe> _recipeDataService;
        private readonly IDataService<RecipeImage> _imageDataService;
        private List<RecipeCategory> _categories;
        private List<Recipe> _recipes;
        private Lazy<Task> _categoryLazy;

        public IEnumerable<RecipeCategory> RecipeCategories => _categories;
        public IEnumerable<Recipe> Recipes => _recipes;
        public RecipeImage? Image {  get; set; }

        public event Action<RecipeCategory>? CategoryCreated;
        public event Action<RecipeCategory>? CategoryUpdated;

        public event Action<Recipe, RecipeCategory>? RecipeCreated;

        public RecipeBookStore(RecipeBook recipeBook, RecipeStore recipeStore, 
            IDataService<RecipeCategory> categoryDataService, IDataService<Recipe> recipeDataService, IDataService<RecipeImage> imageDataService)
        {
            _recipeBook = recipeBook;
            _recipeStore = recipeStore;
            _categoryDataService = categoryDataService;
            _categories = new List<RecipeCategory>();
            _recipes = new List<Recipe>();
            _categoryLazy = new Lazy<Task>(InitializeCategories);
            _recipeDataService = recipeDataService;
            _imageDataService = imageDataService;

            recipeStore.RecipeChanged += OnRecipeChanged;
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

        public async Task<RecipeImage> GetImage(Guid recipeId)
        {
            return await _imageDataService.Get(recipeId);
        }

        public async Task CreateCategory(RecipeCategory category)
        {
            await _categoryDataService.Create(category);
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
            await _recipeDataService.Create(recipe);
            category.AddRecipe(recipe);

            OnRecipeCreated(recipe, category);
        }

        public async Task CreateImage(RecipeImage image)
        {
            await _imageDataService.Create(image);
            Image = image;
        }

        public async Task<bool> DeleteCategory(Guid Id)
        {
            if (await _categoryDataService.Delete(Id)) {
                _categories.Remove(_categories.First(c => c.CategoryId == Id));
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRecipe(Guid Id, RecipeCategory category)
        {
            if (await _recipeDataService.Delete(Id))
            {
                category.RemoveRecipe(Id);
                return true;
            }
            return false;
        }

        //public async Task<bool> DeleteImage(Guid Id)
        //{
        //    if (await _categoryDataService.Delete(Id))
        //    {
        //        _categories.Remove(_categories.First(c => c.CategoryId == Id));
        //        return true;
        //    }
        //    return false;
        //}

        public async Task<bool> UpdateCategory(Guid Id, RecipeCategory category)
        {
            try
            {
                RecipeCategory updatedCategory = await _categoryDataService.Update(Id, category);
                _categories.First(c => c.CategoryId == Id).Name = updatedCategory.Name;
                OnCategoryUpdated(category);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateRecipe(Guid Id, Recipe recipe, RecipeCategory category)
        {
            try
            {
                Recipe updatedRecipe = await _recipeDataService.Update(Id, recipe);
                category.RemoveRecipe(Id);
                category.AddRecipe(updatedRecipe);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> UpdateImage(Guid RecipeId, RecipeImage image)
        {
            try
            {
                RecipeImage updatedImage = await _imageDataService.Update(RecipeId, image);
                Image = updatedImage;
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        private async void OnRecipeChanged(Recipe? recipe)
        {
            if (recipe != null)
                Image = await GetImage(recipe.RecipeId);
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            CategoryCreated?.Invoke(category);
        }

        private void OnCategoryUpdated(RecipeCategory category)
        {
            CategoryUpdated?.Invoke(category);
        }

        private void OnRecipeCreated(Recipe recipe, RecipeCategory category)
        {
            RecipeCreated?.Invoke(recipe, category);
        }

        private async Task InitializeCategories()
        {
            IEnumerable<RecipeCategory> recipeCategories = await _categoryDataService.GetAll();

            _categories.Clear();
            _categories.AddRange(recipeCategories);
        }
    }
}
