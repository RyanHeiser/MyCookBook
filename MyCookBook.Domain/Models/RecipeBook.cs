using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Domain.Models
{
    public class RecipeBook : DomainObject
    {
        public string Name { get; set; }

        public int CategoryCount { get; set; }
        public int RecipeCount { get; set; }

        public List<RecipeCategory> _categories;
        public IEnumerable<RecipeCategory> Categories => _categories;

        public RecipeBook(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            CategoryCount = 0;
            RecipeCount = 0;
            _categories = new List<RecipeCategory>();
        }

        public RecipeBook(string name, int categoryCount, int recipeCount)
        {
            Id = Guid.NewGuid();
            Name = name;
            CategoryCount = categoryCount;
            RecipeCount = recipeCount;
            _categories = new List<RecipeCategory>();
        }

        /// <summary>
        /// Adds a category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(RecipeCategory category)
        {
            _categories.Add(category);
            CategoryCount++;
            RecipeCount += category.RecipeCount;
            category.RecipeCountChanged += OnRecipeCountChanged;
        }

        private void OnRecipeCountChanged(int change)
        {
            RecipeCount += change;
        }


        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="Id">The Id of the category to update.</param>
        /// <param name="recipe">The new category replacing the old value</param>
        /// <returns>True if successful</returns>
        public bool UpdateCategory(Guid Id, RecipeCategory updatedCategory)
        {
            RecipeCategory? existing = Categories.FirstOrDefault(c => c.Id == Id);

            if (existing != null)
            {
                _categories.Remove(existing);
                updatedCategory.Id = Id;
                _categories.Add(updatedCategory);

                RecipeCount += updatedCategory.RecipeCount - existing.RecipeCount;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a category by id.
        /// </summary>
        /// <param name="id">The id of the category to remove.</param>
        public bool RemoveCategory(Guid Id)
        {
            RecipeCategory? existing = Categories.FirstOrDefault(c => c.Id == Id);

            if (existing != null)
            {
                _categories.Remove(existing);
                CategoryCount--;
                RecipeCount -= existing.RecipeCount;
                return true;
            }
            return false;
        }

        public void ClearRecipeCategories()
        {
            _categories.Clear();
            CategoryCount = 0;
            RecipeCount = 0;
        }
    }
}
