using MyCookBook.Domain.Models;
using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Services.Navigation;
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
        private readonly RecipeBookStore _recipeBookStore;

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

        public ICommand LoadBooksCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SelectBookCommand { get; }
        public ICommand UpdateBookCommand { get; }
        public ICommand DeleteBookCommand { get; }

        public RecipeBookListingViewModel(RecipeBookStore recipeBookStore,
            INavigationService createBookNavigationService, INavigationService categoryListingNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _books = new ObservableCollection<RecipeBookViewModel>();

            LoadBooksCommand = new LoadCommand<RecipeBook>(this, recipeBookStore);
            AddCommand = new NavigateCommand(createBookNavigationService); 
            SelectBookCommand = new NavigateCommand(categoryListingNavigationService);
            UpdateBookCommand = new CompositeCommand(new SetCurrentStoreCommand<RecipeBook>(recipeBookStore), new NavigateCommand(createBookNavigationService));
            DeleteBookCommand = new CompositeCommand(new DeleteCommand<RecipeBook>(recipeBookStore), LoadBooksCommand);

            LoadBooksCommand.Execute(null);

            _books.CollectionChanged += OnBookCreated;
            _recipeBookStore.NewCreated += OnBookCreated;
            _recipeBookStore.ItemUpdated += OnBookUpdated;
        }

        public void UpdateBooks()
        {
            _books.Clear();

            foreach (RecipeBook book in _recipeBookStore.Items)
            {
                RecipeBookViewModel bookViewModel = new RecipeBookViewModel(book);
                _books.Add(bookViewModel);
            }
        }

        public override void Dispose()
        {
            _recipeBookStore.NewCreated -= OnBookCreated;
            _recipeBookStore.ItemUpdated -= OnBookUpdated;
            base.Dispose();
        }

        public override void Update()
        {
            UpdateBooks();
            base.Update();
        }

        private void OnBookCreated(RecipeBook book)
        {
            LoadBooksCommand.Execute(null);
        }

        private void OnBookCreated(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasBooks));
        }

        private void OnBookUpdated(RecipeBook book)
        {
            LoadBooksCommand?.Execute(null);
        }
    }
}
