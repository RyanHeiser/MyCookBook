using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Models
{
    public class RecipeBook
    {
        public List<RecipeCategory> _categories;
        public IEnumerable<RecipeCategory> Categories => _categories;

        public RecipeBook(List<RecipeCategory> categories)
        {
            _categories = categories;
        }

        /// <summary>
        /// Gets all the categories in the ReservationBook.
        /// TODO: Implement database to pull from
        /// </summary>
        /// <returns>An IEnumerable comtaining the categories</returns>
        public IEnumerable<RecipeCategory> GetAllCategories()
        {
            return _categories;
        }

        /// <summary>
        /// Adds a category in alphabetical order.
        /// </summary>
        /// <param name="recipe">The recipe to add.</param>
        /// <returns>The index at which the category was inserted</returns>
        public int AddRecipeCategory(RecipeCategory category)
        {
            if (_categories.Count == 0)
            {
                _categories.Add(category);
                return 0;
            }

            for (int i = 0; i < _categories.Count; i++)
            {
                if (string.Compare(_categories[i].Name, category.Name, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    _categories.Insert(i, category);
                    return i;
                }
            }
            return -1;
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
