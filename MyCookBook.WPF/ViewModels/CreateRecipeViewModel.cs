using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Exceptions;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services;
using MyCookBook.WPF.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MyCookBook.WPF.Stores.RecipeStores;

namespace MyCookBook.WPF.ViewModels
{
    public class CreateRecipeViewModel : ViewModelBase
    {
        private RecipeStoreBase<RecipeImage> _imageStore;

        protected string? _name;
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CanCreateRecipe));
            }
        }

        protected int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
                OnPropertyChanged(nameof(Minutes));
                OnPropertyChanged(nameof(CanCreateRecipe));
            }
        }

        protected int _hours;
        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
                OnPropertyChanged(nameof(Hours));
                OnPropertyChanged(nameof(CanCreateRecipe));
            }
        }

        protected int _servings;
        public int Servings
        {
            get
            {
                return _servings;
            }
            set
            {
                _servings = value;
                OnPropertyChanged(nameof(Servings));
                OnPropertyChanged(nameof(CanCreateRecipe));
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private byte[]? _rawThumbnailData;
        public byte[]? RawThumbnailData
        {
            get { return _rawThumbnailData; }
            set
            {
                if (value != _rawThumbnailData)
                {
                    _rawThumbnailData = value;
                    OnPropertyChanged(nameof(RawThumbnailData));
                    OnPropertyChanged(nameof(HasImage));
                }
            }
        }

        private byte[]? _rawImageData;
        public byte[]? RawImageData
        {
            get { return _rawImageData; }
            set
            {
                if (value != _rawImageData)
                {
                    _rawImageData = value;
                    OnPropertyChanged(nameof(RawImageData));
                    OnPropertyChanged(nameof(HasImage));
                }
            }
        }

        public ObservableCollection<StringViewModel> Ingredients { get; set; }

        public ObservableCollection<StringViewModel> Directions { get; set; }

        public bool CanCreateRecipe => HasName && MinutesGreaterThanZero && ServingsGreaterThanZero;
        private bool HasName => !string.IsNullOrEmpty(Name);
        private bool MinutesGreaterThanZero => Minutes > 0;
        private bool ServingsGreaterThanZero => Servings > 0;

        private bool _isSubmitting;
        public bool IsSubmitting
        {
            get
            {
                return _isSubmitting;
            }
            set
            {
                _isSubmitting = value;
                OnPropertyChanged(nameof(IsSubmitting));
            }
        }

        private bool _isEditing = false;
        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            private set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public bool HasImage => _rawImageData != null && _rawThumbnailData != null;

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand UploadImageCommand { get; }
        public ICommand RemoveImageCommand { get; }

        public ICommand AddIngredient {  get; }
        public ICommand AddDirection { get; }

        public CreateRecipeViewModel(RecipeStoreBase<RecipeCategory> categoryStore, RecipeStoreBase<Recipe> recipeStore, RecipeStoreBase<RecipeImage> imageStore,
            INavigationService recipeDisplayNavigationService, INavigationService previousNavigationService)
        {
            imageStore.FinishedLoading += OnImageLoaded;

            _imageStore = imageStore;

            Recipe = recipeStore.Current;
            Category = categoryStore.Current;

            if (recipeStore.Current == null)
            {
                Ingredients = new ObservableCollection<StringViewModel>() { new StringViewModel("") };
                Directions = new ObservableCollection<StringViewModel>() { new StringViewModel("") };

                SubmitCommand = new CompositeCommand(new CreateRecipeCommand(this, categoryStore, recipeStore, imageStore), new NavigateCommand(recipeDisplayNavigationService));
            }
            else
            {
                imageStore.Load();

                Name = recipeStore.Current.Name;
                Minutes = recipeStore.Current.Minutes;
                Servings = recipeStore.Current.Servings;
                Description = recipeStore.Current.Description;
                RawThumbnailData = recipeStore.Current.RawThumbnailData;
                //RawImageData = imageStore.Items.First().RawImageData;
                Ingredients = new ObservableCollection<StringViewModel>(recipeStore.Current.Ingredients.Select(i => new StringViewModel(i)));
                Directions = new ObservableCollection<StringViewModel>(recipeStore.Current.Directions.Select(d => new StringViewModel(d)));

                SubmitCommand = new CompositeCommand(new UpdateRecipeCommand(this, recipeStore, imageStore), new NavigateCommand(recipeDisplayNavigationService));

                IsEditing = true;
            }

            CancelCommand = new NavigateCommand(previousNavigationService);

            UploadImageCommand = new UploadImageCommand(this);
            RemoveImageCommand = new RemoveImageCommand(this);

            AddIngredient = new AddToCollectionCommand<StringViewModel>(Ingredients, () => new StringViewModel(""));
            AddDirection = new AddToCollectionCommand<StringViewModel>(Directions, () => new StringViewModel(""));
        }

        private void OnImageLoaded()
        {
            RawImageData = _imageStore.Items.First().RawImageData;
        }
    }
}
