using MyCookBook.Commands;
using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public bool HasRecipes => Recipes.Any();

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
                _recipeStore.CurrentRecipe = _selectedRecipe?.Recipe; // updates the current recipe store when list selection is changed
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

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
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
            _recipes = new ObservableCollection<RecipeViewModel>(_recipeStore.CurrentCategory?.GetAllRecipes().Select(r => new RecipeViewModel(r)));

            Category = recipeStore.CurrentCategory;
            Name = Category?.Name ?? "New Category";

            AddCommand = new NavigateCommand<CreateRecipeViewModel>(createRecipeNavigationService, recipeStore);
            SelectRecipeCommand = new NavigateCommand<RecipeDisplayViewModel>(recipeDisplayNavigationService, recipeStore);

            _recipeBookStore.RecipeCreated += OnRecipeCreated;
            _recipes.CollectionChanged += OnRecipeCreated;
        }

        /// <summary>
        /// Unsubscribe from RecipeCreated event so that this view model can properly be disposed
        /// </summary>
        public override void Dispose()
        {
            _recipeBookStore.RecipeCreated -= OnRecipeCreated;
            base.Dispose();
        }

        // NOT CURRENTLY IN USE
        private void OnRecipeCreated(Recipe recipe, RecipeCategory category)
        {
            RecipeViewModel viewModel = new RecipeViewModel(recipe);
            _recipes.Add(viewModel);
        }

        private void OnRecipeCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasRecipes));
        }

        //public static RecipeListingViewModel LoadViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore,
        //    NavigationService<CreateRecipeViewModel> createRecipeNavigationService, NavigationService<RecipeDisplayViewModel> recipeDisplayNavigationService)
        //{

        //}
    }
}
