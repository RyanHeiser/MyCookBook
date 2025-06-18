using MyCookBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        public string Name => Category?.Name ?? "New Category";
        public int Count => Category?.Recipes?.Count() ?? 0;

        public CategoryViewModel(RecipeCategory category)
        {
            Category = category;
        }
    }
}
