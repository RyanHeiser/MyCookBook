using MyCookBook.WPF.Commands;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.Stores;

namespace MyCookBook.WPF.ViewModels
{
	public class RecipeListingViewModel : ViewModelBase
    {
        private ObservableCollection<RecipeViewModel> _recipes;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;
        private readonly RecipeStoreBase<Recipe> _recipeStore;

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
                _recipeStore.Current = _selectedRecipe?.Recipe; // updates the current recipe store when list selection is changed
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

        //private bool _isLoading;
        //public bool IsLoading
        //{
        //    get
        //    {
        //        return _isLoading;
        //    }
        //    set
        //    {
        //        _isLoading = value;
        //        OnPropertyChanged(nameof(IsLoading));
        //    }
        //}

        public ICommand LoadRecipesCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand AddCommand { get; }

        public ICommand MoveCategoryCommand { get; }
        public ICommand RenameCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public ICommand SelectRecipeCommand { get; }
        public ICommand MoveRecipeCommand { get; }
        public ICommand EditRecipeCommand { get; }
        public ICommand DeleteRecipeCommand { get; }

        public RecipeListingViewModel(RecipeStoreBase<RecipeCategory> categoryStore, RecipeStoreBase<Recipe> recipeStore, DeleteStore deleteStore,
            MoveStore moveStore, INavigationService createRecipeNavigationService, INavigationService createCategoryNavigationService, 
            INavigationService recipeDisplayNavigationService, INavigationService deleteRecipeNavigationService, 
            INavigationService deleteCategoryNavigationService, INavigationService previousNavigationService)
        {
            _categoryStore = categoryStore;
            _recipeStore = recipeStore;
            _recipes = new ObservableCollection<RecipeViewModel>();

            Category = categoryStore.Current;
            Name = categoryStore.Current?.Name ?? "New Category";

            LoadRecipesCommand = new LoadCommand<Recipe>(this, recipeStore);
            BackCommand = new NavigateCommand(previousNavigationService);
            AddCommand = new NavigateCommand(createRecipeNavigationService);

            MoveCategoryCommand = new StartMoveCommand<RecipeCategory>(moveStore, categoryStore.Current);
            RenameCategoryCommand = new NavigateCommand(createCategoryNavigationService);
            DeleteCategoryCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteCategoryNavigationService));

            SelectRecipeCommand = new NavigateCommand(recipeDisplayNavigationService);
            EditRecipeCommand = new CompositeCommand(new SetCurrentStoreCommand<Recipe>(recipeStore), new NavigateCommand(createRecipeNavigationService));
            MoveRecipeCommand = new StartMoveCommand<Recipe>(moveStore);
            DeleteRecipeCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteRecipeNavigationService));

            LoadRecipesCommand.Execute(null);

            _recipes.CollectionChanged += OnRecipeCreated;
            _categoryStore.ItemUpdated += OnCategoryUpdated;
            _recipeStore.ItemDeleted += OnRecipeDeleted;
        }

        public override void Dispose()
        {
            _categoryStore.ItemUpdated -= OnCategoryUpdated;
            _categoryStore.ItemDeleted -= OnRecipeDeleted;
            base.Dispose();
        }

        public void UpdateRecipes()
        {
            _recipes.Clear();

            foreach (Recipe recipe in _recipeStore.Items)
            {
                RecipeViewModel recipeViewModel = new RecipeViewModel(recipe);
                _recipes.Add(recipeViewModel);
            }
        }

        public override void Update()
        {
            UpdateRecipes();
            Name = _categoryStore.Current?.Name ?? "New Category";
            base.Update();
        }

        public void UpdateRecipes(IEnumerable<Recipe> recipes)
        {
            _recipes.Clear();

            foreach (Recipe recipe in recipes)
            {
                RecipeViewModel recipeViewModel = new RecipeViewModel(recipe);
                _recipes.Add(recipeViewModel);
            }
            OnPropertyChanged(nameof(HasRecipes));
        }

        private void OnRecipeCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasRecipes));
        }

        private void OnCategoryUpdated(RecipeCategory category)
        {
            if (category.Id == _categoryStore.Current.Id)
            {
                Name = category.Name; 
            }
        }

        private void OnRecipeDeleted()
        {
            UpdateRecipes();
        }
    }
}
