using MyCookBook.WPF.Commands;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyCookBook.WPF.Stores.RecipeStores;
using System.Xml.Linq;
using System.Security.RightsManagement;
using MyCookBook.WPF.Stores;

namespace MyCookBook.WPF.ViewModels
{
    public class CategoryListingViewModel : ViewModelBase
    {
        private ObservableCollection<CategoryViewModel> _categories;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly RecipeStoreBase<RecipeCategory> _categoryStore;

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
                _categoryStore.Current = _selectedCategory?.Category; // updates the current category store when list selection is changed
                OnPropertyChanged(nameof(SelectedCategory));

                SelectCategoryCommand.Execute(null);
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

        public ICommand LoadCategoriesCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand BackCommand { get; }

        public ICommand UpdateRecipeBookCommand { get; }
        public ICommand DeleteRecipeBookCommand { get; }

        public ICommand AddCommand { get; }
        public ICommand MoveCategoryCommand { get; }
        public ICommand RenameCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }

        public CategoryListingViewModel(RecipeStoreBase<RecipeBook> recipeBookStore, RecipeStoreBase<RecipeCategory> categoryStore, 
            DeleteStore deleteStore, MoveStore moveStore, INavigationService createCategoryNavigationService, 
            INavigationService createRecipeBookNavigationService, INavigationService recipeListingNavigationService, 
            INavigationService deleteCategoryNavigationService, INavigationService deleteBookNavigationService, INavigationService previousNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _categoryStore = categoryStore;
            _categories = new ObservableCollection<CategoryViewModel>();

            Book = recipeBookStore.Current;

            Name = recipeBookStore.Current?.Name ?? "New Recipe Book";

            LoadCategoriesCommand = new LoadCommand<RecipeCategory>(this, categoryStore);
            SelectCategoryCommand = new NavigateCommand(recipeListingNavigationService);
            BackCommand = new NavigateCommand(previousNavigationService);

            UpdateRecipeBookCommand = new NavigateCommand(createRecipeBookNavigationService);
            DeleteRecipeBookCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteBookNavigationService));

            AddCommand = new NavigateCommand(createCategoryNavigationService);
            MoveCategoryCommand = new StartMoveCommand<RecipeCategory>(moveStore);
            RenameCategoryCommand = new CompositeCommand(new SetCurrentStoreCommand<RecipeCategory>(categoryStore), new NavigateCommand(createCategoryNavigationService));
            DeleteCategoryCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteCategoryNavigationService));

            LoadCategoriesCommand.Execute(null);

            _categories.CollectionChanged += OnCategoryCreated;
            _categoryStore.NewCreated += OnCategoryCreated;
            _categoryStore.ItemUpdated += OnCategoryUpdated;
            _categoryStore.ItemDeleted += OnCategoryDeleted;
            _recipeBookStore.ItemUpdated += OnBookUpdated;
        }

        /// <summary>
        /// Updates list of RecipeCategories.
        /// </summary>
        public void UpdateCategories()
        {
            _categories.Clear();

            foreach (RecipeCategory category in _categoryStore.Items)
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel(category);
                _categories.Add(categoryViewModel);
            }

            OnPropertyChanged(nameof(HasCategories));
        }

        /// <summary>
        /// Updates the view model.
        /// </summary>
        public override void Update()
        {
            UpdateCategories();
            Name = _recipeBookStore.Current.Name;
        }

        /// <summary>
        /// Disposes of the view model.
        /// </summary>
        public override void Dispose()
        {
            _categoryStore.NewCreated -= OnCategoryCreated;
            _categoryStore.ItemUpdated -= OnCategoryUpdated;
            _categoryStore.ItemDeleted -= OnCategoryDeleted;
            _recipeBookStore.ItemUpdated -= OnBookUpdated;
            base.Dispose();
        }

        private void OnCategoryCreated(RecipeCategory category)
        {
            UpdateCategories();
        }

        private void OnCategoryCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasCategories));
        }

        /// <summary>
        /// Updates the title when RecipeBook name is updated.
        /// </summary>
        /// <param name="book"></param>
        private void OnBookUpdated(RecipeBook book)
        {
            if (book.Id == _recipeBookStore?.Current?.Id)
            {
                Name = book.Name;
            }
        }

        private void OnCategoryUpdated(RecipeCategory category)
        {
            UpdateCategories();
        }

        private void OnCategoryDeleted()
        {
            UpdateCategories();
        }

    }
}
