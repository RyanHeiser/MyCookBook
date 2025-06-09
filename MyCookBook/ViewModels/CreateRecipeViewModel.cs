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

        public ICommand SubmitCommand { get; set; }

        // temp recipe
        public Recipe TempRecipe { get; }

        public CreateRecipeViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, NavigationService<RecipeDisplayViewModel> navigationService)
        {
            _recipe = recipeStore.CurrentRecipe;
            _category = recipeStore.CurrentCategory;

            SubmitCommand = new NavigateCommand<RecipeDisplayViewModel>(navigationService, recipeStore);
        }
    }
}
