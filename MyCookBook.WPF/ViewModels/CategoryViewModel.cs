using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        public new RecipeCategory Category { get; set; }
        public string Name => Category.Name;
        public int Count => Category.RecipeCount;

        public CategoryViewModel(RecipeCategory category)
        {
            Category = category;
        }
    }
}
