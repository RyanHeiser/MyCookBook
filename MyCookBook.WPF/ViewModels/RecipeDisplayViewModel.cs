using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Exceptions;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
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
