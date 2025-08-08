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
        public string Hours => Recipe?.Hours.ToString() ?? String.Empty;
        public string Minutes => Recipe?.Minutes.ToString() ?? String.Empty;
        public string Servings => Recipe?.Servings > 0 ? Recipe.Servings.ToString() : String.Empty;
        public string Description => Recipe?.Description ?? String.Empty;
        public byte[]? RawThumbnailData => Recipe?.RawThumbnailData;
        public IEnumerable<string> Ingredients => Recipe?.Ingredients ?? new List<string>();
        public IEnumerable<string> Directions => Recipe?.Directions ?? new List<string>();

        public bool HasTime => IsNotZeroOrEmpty(Hours) || IsNotZeroOrEmpty(Minutes);
        public bool HasServings => IsNotZeroOrEmpty(Servings);

        public RecipeViewModel(Recipe? recipe)
        {
            Recipe = recipe;
        }

        private static bool IsNotZeroOrEmpty(string value)
        {
            return !value.Equals("0") && !String.IsNullOrEmpty(value);
        }
    }
}
