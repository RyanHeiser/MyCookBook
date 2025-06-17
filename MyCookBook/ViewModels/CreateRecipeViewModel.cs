using MyCookBook.Commands;
using MyCookBook.Exceptions;
using MyCookBook.Models;
using MyCookBook.Services;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
    public class CreateRecipeViewModel : ViewModelBase
    {
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

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand AddIngredient {  get; }
        public ICommand AddDirection { get; }

        public CreateRecipeViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, 
            NavigationService<RecipeListingViewModel> recipeListingNavigationService, NavigationService<RecipeDisplayViewModel> recipeDisplayNavigationService)
        {
            Recipe = recipeStore.CurrentRecipe;
            Category = recipeStore.CurrentCategory;

            if (recipeStore.CurrentRecipe == null)
            {
                Ingredients = new ObservableCollection<StringViewModel>() { new StringViewModel("") };
                Directions = new ObservableCollection<StringViewModel>() { new StringViewModel("") };
            } 
            else
            {
                Ingredients = new ObservableCollection<StringViewModel>(recipeStore.CurrentRecipe.Ingredients.Select(i => new StringViewModel(i)));
                Directions = new ObservableCollection<StringViewModel>(recipeStore.CurrentRecipe.Directions.Select(d => new StringViewModel(d)));
            }

            SubmitCommand = new CreateRecipeCommand(this, recipeBookStore, recipeStore, recipeDisplayNavigationService);
            CancelCommand = new NavigateCommand<RecipeListingViewModel>(recipeListingNavigationService, recipeStore);

            AddIngredient = new AddToCollectionCommand<StringViewModel>(Ingredients, () => new StringViewModel(""));
            AddDirection = new AddToCollectionCommand<StringViewModel>(Directions, () => new StringViewModel(""));
        }
    }
}
