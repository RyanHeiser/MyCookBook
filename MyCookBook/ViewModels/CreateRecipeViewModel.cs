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
            }
        }

        protected readonly ObservableCollection<string> _ingredients;
        public IEnumerable<string> Ingredients => _ingredients;

        protected readonly ObservableCollection<string> _directions;
        public IEnumerable<string> Directions => _directions;

        public bool CanCreateRecipe => true;

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

        public CreateRecipeViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, 
            NavigationService<RecipeListingViewModel> recipeListingNavigationService, NavigationService<RecipeDisplayViewModel> recipeDisplayNavigationService)
        {
            Recipe = recipeStore.CurrentRecipe;
            Category = recipeStore.CurrentCategory;

            SubmitCommand = new CreateRecipeCommand(this, recipeBookStore, recipeStore, recipeDisplayNavigationService);
            CancelCommand = new NavigateCommand<RecipeListingViewModel>(recipeListingNavigationService, recipeStore);
        }
    }
}
