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
		public ICommand EditCommand { get; }
		public ICommand DeleteCommand { get; }

        public RecipeDisplayViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, 
			INavigationService createRecipeNavigationService, INavigationService previousNavigationService)
        {
			Recipe = recipeStore.CurrentRecipe;
			Category = recipeStore.CurrentCategory;

            BackCommand = new NavigateCommand(previousNavigationService);
			EditCommand = new NavigateCommand(createRecipeNavigationService);
			DeleteCommand = new CompositeCommand(new DeleteRecipeCommand(recipeBookStore, Category), BackCommand);

            RecipeViewModel = new RecipeViewModel(recipeStore.CurrentRecipe);
        }

    }
}
