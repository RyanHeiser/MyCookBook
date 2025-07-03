using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.ViewModels
{
    public class RecipeViewModel : ViewModelBase
    {
        public string Name => Recipe?.Name ?? "New Recipe";
        public string Minutes => Recipe?.Minutes > 0 ? Recipe.Minutes.ToString() : String.Empty;
        public string Servings => Recipe?.Servings > 0 ? Recipe.Servings.ToString() : String.Empty;
        public byte[]? RawThumbnailData => Recipe?.RawThumbnailData;
        public IEnumerable<string> Ingredients => Recipe?.Ingredients ?? new List<string>();
        public IEnumerable<string> Directions => Recipe?.Directions ?? new List<string>();

        public RecipeViewModel(Recipe? recipe)
        {
            Recipe = recipe;
        }
    }
}
