using MyCookBook.Commands;
using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
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
            INavigationService createCategoryNavigationService, INavigationService recipeListingNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _categories = new ObservableCollection<CategoryViewModel>(_recipeBookStore.RecipeCategories.Select(c => new CategoryViewModel(c)));

            AddCommand = new NavigateCommand(createCategoryNavigationService);
            SelectCategoryCommand = new NavigateCommand(recipeListingNavigationService);

            _categories.CollectionChanged += OnCategoryCreated;
            _recipeBookStore.CategoryCreated += OnCategoryCreated;
        }

        public override void Dispose()
        {
            _recipeBookStore.CategoryCreated -= OnCategoryCreated;
            base.Dispose();
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            _categories.Add(new CategoryViewModel(category));
        }

        private void OnCategoryCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasCategories));
        }
    }
}
