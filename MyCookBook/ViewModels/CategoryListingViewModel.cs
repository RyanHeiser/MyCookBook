using MyCookBook.Commands;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
    public class CategoryListingViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryViewModel> _categories;
        private readonly RecipeBookStore _recipeBookStore;
        private readonly RecipeStore _recipeStore;

        public IEnumerable<CategoryViewModel> Categories => _categories;

        public bool HasCategories => Categories.Any();

        private CategoryViewModel? _selectedCategory;
        public CategoryViewModel? SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                _recipeStore.CurrentCategory = _selectedCategory?.Category; // updates the current category store when list selection is changed
                OnPropertyChanged(nameof(SelectedCategory));
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

        public ICommand AddCommand { get; }
        public ICommand SelectCategoryCommand { get; }

        public CategoryListingViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore,
            NavigationService<CreateRecipeViewModel> createRecipeNavigationService, NavigationService<RecipeListingViewModel> recipeListingNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _categories = new ObservableCollection<CategoryViewModel>(_recipeBookStore.RecipeCategories.Select(c => new CategoryViewModel(c)));

            //AddCommand = new NavigateCommand<CreateRecipeViewModel>(createRecipeNavigationService, recipeStore);
            SelectCategoryCommand = new NavigateCommand<RecipeListingViewModel>(recipeListingNavigationService, recipeStore);

            _categories.CollectionChanged += OnCategoryCreated;
        }

        private void OnCategoryCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasCategories));
        }
    }
}
