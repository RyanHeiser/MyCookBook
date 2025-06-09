using MyCookBook.Commands;
using MyCookBook.Exceptions;
using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
    public class RecipeDisplayViewModel : ViewModelBase, IParameterNavigationService
    {
        public RecipeViewModel? Recipe { get; set; }
        public RecipeCategory? Category { get; set; }
        public ICommand BackCommand { get; }

        public RecipeDisplayViewModel(RecipeBookStore recipeBookStore, NavigationService<CreateRecipeViewModel> navigationService)
        {
            BackCommand = new NavigateCommand<CreateRecipeViewModel>(navigationService);
        }

        public void ParameterInitialize(params object[] parameters)
        {
            if (parameters.Length > 1)
            {
                try
                {
                    Recipe = new RecipeViewModel((Recipe) parameters[0]);
                    Category = (RecipeCategory)parameters[1];
                }
                catch { }
            }
            else
            {
                throw new InvalidParametersException<RecipeDisplayViewModel>(parameters);
            }
        }
    }
}
