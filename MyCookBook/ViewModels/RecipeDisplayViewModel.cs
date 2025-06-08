using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    class RecipeDisplayViewModel : ViewModelBase
    {
		private string _name;
		public string Name
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

		private int _minutes;
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

		private int _servings;
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

		private readonly ObservableCollection<string> _ingredients;
		public IEnumerable<string> Ingredients => _ingredients;

		private readonly ObservableCollection<string> _directions;
		public IEnumerable<string> Directions => _directions;

        public RecipeDisplayViewModel(IEnumerable<string> ingredients, IEnumerable<string> directions)
        {
			_ingredients = new ObservableCollection<string>(ingredients);
			_directions = new ObservableCollection<string>(directions);
        }
    }
}
