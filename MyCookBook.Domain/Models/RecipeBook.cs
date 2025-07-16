using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MyCookBook.Domain.Models
{
    public class RecipeBook : DomainObject
    {
        public string Name { get; set; }
        [JsonInclude]
        public List<RecipeCategory> _categories;
        [JsonIgnore]
        public IEnumerable<RecipeCategory> Categories => _categories;

        public RecipeBook(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            _categories = new List<RecipeCategory>();
        }


        /// <summary>
        /// Adds a category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        public void AddCategory(RecipeCategory category)
        {
            _categories.Add(category);
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
                return true;
            }
            return false;
        }

        public void ClearRecipeCategories()
        {
            _categories.Clear();
        }
    }
}
