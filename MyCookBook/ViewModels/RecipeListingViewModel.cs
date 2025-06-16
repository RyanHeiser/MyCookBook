using MyCookBook.Commands;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
	public class RecipeListingViewModel : ViewModelBase
    {
        private ObservableCollection<RecipeViewModel> _recipes;
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;

        public IEnumerable<RecipeViewModel> Recipes => _recipes;

        private RecipeViewModel? _selectedRecipe;
        public RecipeViewModel? SelectedRecipe
        {
            get
            {
                return _selectedRecipe;
            }
            set
            {
                _selectedRecipe = value;
                _recipeStore.CurrentRecipe = _selectedRecipe?.Recipe;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

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

        public ICommand BackCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SelectRecipeCommand { get; }

        public RecipeListingViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, 
            NavigationService<CreateRecipeViewModel> createRecipeNavigationService, NavigationService<RecipeDisplayViewModel> recipeDisplayNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _recipes = new ObservableCollection<RecipeViewModel>();

            Category = recipeStore.CurrentCategory;
            Name = Category?.Name ?? "New Category";

            AddCommand = new NavigateCommand<CreateRecipeViewModel>(createRecipeNavigationService, recipeStore);
            SelectRecipeCommand = new NavigateCommand<RecipeDisplayViewModel>(recipeDisplayNavigationService, recipeStore);
        }
    }
}
