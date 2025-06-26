using MyCookBook.WPF.Commands;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
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
                if (_selectedRecipe == value) 
                    return;

                _selectedRecipe = value;
                _recipeStore.CurrentRecipe = _selectedRecipe?.Recipe; // updates the current recipe store when list selection is changed
                OnPropertyChanged(nameof(SelectedRecipe));

                SelectRecipeCommand.Execute(null);
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

        public ICommand LoadRecipesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SelectRecipeCommand { get; }
        public ICommand DeleteRecipeCommand { get; }

        public RecipeListingViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, 
            INavigationService createRecipeNavigationService, INavigationService recipeDisplayNavigationService,
            INavigationService previousNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _recipes = new ObservableCollection<RecipeViewModel>();

            Category = recipeStore.CurrentCategory;
            Name = Category?.Name ?? "New Category";

            LoadRecipesCommand = new LoadRecipesCommand(this, recipeBookStore);
            BackCommand = new NavigateCommand(previousNavigationService);
            AddCommand = new NavigateCommand(createRecipeNavigationService);
            SelectRecipeCommand = new NavigateCommand(recipeDisplayNavigationService);
            DeleteRecipeCommand = new CompositeCommand(new DeleteRecipeCommand(recipeBookStore, Category), LoadRecipesCommand);

            LoadRecipesCommand.Execute(null);

            _recipes.CollectionChanged += OnRecipeCreated;
        }

        public void UpdateRecipes(IEnumerable<Recipe> recipes)
        {
            _recipes.Clear();

            foreach (Recipe recipe in recipes)
            {
                RecipeViewModel recipeViewModel = new RecipeViewModel(recipe);
                _recipes.Add(recipeViewModel);
            }
        }

        private void OnRecipeCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasRecipes));
        }
    }
}
