using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels
{
    public class RecipeBookListingViewModel : ViewModelBase
    {
        private ObservableCollection<RecipeBookViewModel> _books;
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;

        public IEnumerable<RecipeBookViewModel> Books => _books;

        public bool HasBooks => Books.Any();

        private RecipeBookViewModel? _selectedBook;
        public RecipeBookViewModel? SelectedBook
        {
            get
            {
                return _selectedBook;
            }
            set
            {
                if (_selectedBook == value)
                    return;

                _selectedBook = value;
                _recipeBookStore.Current = _selectedBook.Book; // updates the current book store when list selection is changed
                OnPropertyChanged(nameof(SelectedBook));

                SelectBookCommand.Execute(null);
            }
        }

        public ICommand LoadBooksCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SelectBookCommand { get; }
        public ICommand UpdateBookCommand { get; }
        public ICommand DeleteBookCommand { get; }

        public ICommand ExportCommand { get; }

        public RecipeBookListingViewModel(RecipeStoreBase<RecipeBook> recipeBookStore, DeleteStore deleteStore,
            RecipeBookIODataService IODataServce, INavigationService createBookNavigationService, 
            INavigationService categoryListingNavigationService, INavigationService deleteBookNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _books = new ObservableCollection<RecipeBookViewModel>();

            LoadBooksCommand = new LoadCommand<RecipeBook>(this, recipeBookStore);
            AddCommand = new NavigateCommand(createBookNavigationService); 
            SelectBookCommand = new NavigateCommand(categoryListingNavigationService);
            UpdateBookCommand = new CompositeCommand(new SetCurrentStoreCommand<RecipeBook>(recipeBookStore), new NavigateCommand(createBookNavigationService));
            DeleteBookCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteBookNavigationService));

            ExportCommand = new ExportBookCommand(IODataServce);

            LoadBooksCommand.Execute(null);

            _books.CollectionChanged += OnBookCreated;
            _recipeBookStore.NewCreated += OnBookCreated;
            _recipeBookStore.ItemUpdated += OnBookUpdated;
            _recipeBookStore.ItemDeleted += OnBookDeleted;
        }

        public void UpdateBooks()
        {
            _books.Clear();

            foreach (RecipeBook book in _recipeBookStore.Items)
            {
                RecipeBookViewModel bookViewModel = new RecipeBookViewModel(book);
                _books.Add(bookViewModel);
            }

            OnPropertyChanged(nameof(HasBooks));
        }

        public override void Dispose()
        {
            _recipeBookStore.NewCreated -= OnBookCreated;
            _recipeBookStore.ItemUpdated -= OnBookUpdated;
            _recipeBookStore.ItemDeleted -= OnBookDeleted;
            base.Dispose();
        }

        public override void Update()
        {
            UpdateBooks();
            base.Update();
        }

        private void OnBookCreated(RecipeBook book)
        {
            UpdateBooks();
        }

        private void OnBookCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasBooks));
        }

        private void OnBookUpdated(RecipeBook book)
        {
            UpdateBooks();
        }

        private void OnBookDeleted()
        {
            UpdateBooks();
        }
    }
}
