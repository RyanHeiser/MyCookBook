using MyCookBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    class RecipeViewModelBase : ViewModelBase
    {
        protected string? _name;
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        protected int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
            }
        }

        protected int _servings;
        public int Servings
        {
            get
            {
                return _servings;
            }
            set
            {
                _servings = value;
                OnPropertyChanged(nameof(Servings));
            }
        }

        protected readonly ObservableCollection<string> _ingredients;
        public IEnumerable<string> Ingredients => _ingredients;

        protected readonly ObservableCollection<string> _directions;
        public IEnumerable<string> Directions => _directions;

        public RecipeViewModelBase(Recipe recipe)
        {
            _ingredients = new ObservableCollection<string>(recipe.Ingredients);
            _directions = new ObservableCollection<string>(recipe.Directions);
        }
    }
}
