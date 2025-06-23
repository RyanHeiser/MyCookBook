using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class RecipeStore
    {
        private Recipe? _currentRecipe;
        public Recipe? CurrentRecipe
        {
            get => _currentRecipe;
            set
            {
                _currentRecipe = value;
            }
        }

        private RecipeCategory? _currentCategory;
        public RecipeCategory? CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                OnCategoryChanged(CurrentCategory);
            }
        }

        public event Action<RecipeCategory>? CategoryChanged;

        private void OnCategoryChanged(RecipeCategory category)
        {
            CategoryChanged?.Invoke(category);
        }
    }
}
