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
    public class CreateRecipeViewModel : ViewModelBase, IParameterNavigationService
    {
        //private string? _name = "";
        //public string? Name
        //{
        //    get
        //    {
        //        return _name;
        //    }
        //    set
        //    {
        //        _name = value;
        //        OnPropertyChanged(nameof(Name));
        //    }
        //}

        //private int _minutes = ;
        //public int Minutes
        //{
        //    get
        //    {
        //        return _minutes;
        //    }
        //    set
        //    {
        //        _minutes = value;
        //        OnPropertyChanged(nameof(Minutes));
        //    }
        //}

        //private int _servings;
        //public int Servings
        //{
        //    get
        //    {
        //        return _servings;
        //    }
        //    set
        //    {
        //        _servings = value;
        //        OnPropertyChanged(nameof(Servings));
        //    }
        //}

        //private ObservableCollection<string> _ingredients;
        //public IEnumerable<string> Ingredients
        //{
        //    get => _ingredients;

        //    set
        //    {
        //        _ingredients = new ObservableCollection<string>(value);
        //    }
        //}

        //private ObservableCollection<string> _directions;
        //public IEnumerable<string> Directions
        //{
        //    get => _directions;
            
        //    set
        //    {
        //        _directions = new ObservableCollection<string>(value);
        //    }
        //}

        private RecipeViewModel _recipeVM;
        public RecipeViewModel RecipeVM
        {
            get => _recipeVM;

            set
            {
                OnPropertyChanged(nameof(RecipeVM));
            }
        }

        private Recipe _recipe;
        public Recipe Recipe => _recipe;
        private RecipeCategory _category;
        public RecipeCategory Category => _category;

        private readonly RecipeBookStore _recipeBookStore;

        public object[] NavParameters { get; set; }
        

        public ICommand SubmitCommand { get; set; }

        // temp recipe
        public Recipe TempRecipe { get; }

        public CreateRecipeViewModel(RecipeBookStore recipeBookStore, NavigationService<RecipeDisplayViewModel> navigationService)
        {
            //Ingredients = new ObservableCollection<string>();
            //Directions = new ObservableCollection<string>();

            _recipeBookStore = recipeBookStore;

            //TEMP
            _recipe = new Recipe("Shepherd's pie", 60, 6, new List<string> { "potato", "lamb", "veggies" }, new List<string> { "mash potatoes", "cook lamb", "cook veggies", "combine" });
            _category = new RecipeCategory("Entrees", new List<Recipe>());

            // temp recipe
            _recipeBookStore.CreateRecipeCategory(_category);
            _recipeBookStore.CreateRecipe(_recipe, _category);

            NavParameters = new object[2];
            NavParameters[0] = _recipe;
            NavParameters[1] = _category;

            SubmitCommand = new NavigateCommand<RecipeDisplayViewModel>(navigationService);
        }

        /// <summary>
        /// Sets the recipe fields to those of a RecipeViewModel.
        /// </summary>
        /// <param name="recipe">The RecipeViewModel</param>
        //private void SetRecipe(RecipeViewModel recipe)
        //{
        //    Name = recipe.Name;
        //    Minutes = recipe.Minutes;
        //    Servings = recipe.Servings;
        //    Ingredients = recipe.Ingredients;
        //    Directions = recipe.Directions;
        //}

        
        public void ParameterInitialize(params object[] parameters)
        {
            if (parameters.Length > 1)
            {
                try
                {
                    _recipe = (Recipe) parameters[0];
                    _category = (RecipeCategory) parameters[1];
                } catch { }
                _recipeVM = new RecipeViewModel(_recipe);
                //SetRecipe(_recipeVM);
            } 
            else if (parameters.Length == 1)
            {
                throw new InvalidParametersException<CreateRecipeViewModel>(parameters);
            }
        }
    }
}
