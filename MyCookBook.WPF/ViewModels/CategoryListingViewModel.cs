using MyCookBook.WPF.Commands;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
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
                if (_selectedCategory == value)
                    return;

                _selectedCategory = value;
                _recipeStore.CurrentCategory = _selectedCategory?.Category; // updates the current category store when list selection is changed
                OnPropertyChanged(nameof(SelectedCategory));

                SelectCategoryCommand.Execute(null);
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

        public ICommand LoadCategoriesCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryListingViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore,
            INavigationService createCategoryNavigationService, INavigationService recipeListingNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _recipeStore = recipeStore;
            _categories = new ObservableCollection<CategoryViewModel>();

            LoadCategoriesCommand = new LoadRecipeCategoriesCommand(this, recipeBookStore);
            AddCommand = new NavigateCommand(createCategoryNavigationService);
            SelectCategoryCommand = new NavigateCommand(recipeListingNavigationService);
            DeleteCategoryCommand = new CompositeCommand(new DeleteCategoryCommand(recipeBookStore), LoadCategoriesCommand);

            LoadCategoriesCommand.Execute(null);

            _categories.CollectionChanged += OnCategoryCreated;
            _recipeBookStore.CategoryCreated += OnCategoryCreated;
        }

        public void UpdateCategories(IEnumerable<RecipeCategory> categories)
        {
            _categories.Clear();

            foreach (RecipeCategory category in categories)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                _categories.Add(categoryViewModel);
            }
        }

        public override void Dispose()
        {
            _recipeBookStore.CategoryCreated -= OnCategoryCreated;
            base.Dispose();
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            LoadCategoriesCommand.Execute(null);
        }

        private void OnCategoryCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasCategories));
        }
    }
}
