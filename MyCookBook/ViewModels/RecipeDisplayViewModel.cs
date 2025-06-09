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
    public class RecipeDisplayViewModel : ViewModelBase
    {
        public RecipeViewModel? Recipe { get; set; }
        public RecipeCategory? Category { get; set; }
        public ICommand BackCommand { get; }

        public RecipeDisplayViewModel(RecipeBookStore recipeBookStore, RecipeStore recipeStore, NavigationService<CreateRecipeViewModel> navigationService)
        {
            BackCommand = new NavigateCommand<CreateRecipeViewModel>(navigationService, recipeStore);
        }

    }
}
