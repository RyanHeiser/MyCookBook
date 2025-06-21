using MyCookBook.Commands;
using MyCookBook.Exceptions;
using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
    public class RecipeDisplayViewModel : ViewModelBase
    {
		private RecipeViewModel? _recipeViewModel;
		public RecipeViewModel? RecipeViewModel
        {
			get
			{
				return _recipeViewModel;
			}
			set
			{
				_recipeViewModel = value;
				OnPropertyChanged(nameof(RecipeViewModel));
			}
		}
		public ICommand BackCommand { get; }

        public RecipeDisplayViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, INavigationService recipeListingNavigationService)
        {
            BackCommand = new NavigateCommand(recipeListingNavigationService);

            RecipeViewModel = new RecipeViewModel(recipeStore.CurrentRecipe);
        }

    }
}
