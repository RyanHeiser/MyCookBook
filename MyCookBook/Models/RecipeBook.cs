using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Models
{
    public class RecipeBook
    {
        private List<RecipeCategory> _categories;
        public IEnumerable<RecipeCategory> Categories { get { return _categories; } }

        public RecipeBook(List<RecipeCategory> categories)
        {
            _categories = categories;
        }

        /// <summary>
        /// Adds a category in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        public void AddRecipeCategory(RecipeCategory category)
        {
            _categories.Add(category);
            for (int i = 0; i < _categories.Count; i++)
            {
                if (string.Compare(_categories[i].Name, category.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    _categories.Insert(i, category);
                }
            }
        }

        /// <summary>
        /// Removes a category by id.
        /// </summary>
        /// <param name="id">The id of the recipe to remove.</param>
        public void RemoveRecipeCategory(Guid id)
        {
            foreach (RecipeCategory category in _categories)
            {
                if (category.Id == id)
                {
                    _categories.Remove(category);
                }
            }
        }

        /// <summary>
        /// Clears the recipes.
        /// </summary>
        public void ClearRecipeCategories()
        {
            _categories.Clear();
        }
    }
}
