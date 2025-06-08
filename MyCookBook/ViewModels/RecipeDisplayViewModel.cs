using MyCookBook.Commands;
using MyCookBook.Models;
using MyCookBook.Services;
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
        public RecipeViewModel Recipe { get; set; }
        public ICommand BackCommand { get; }

        public RecipeDisplayViewModel(Recipe recipe, NavigationService navigationService)
        {
            Recipe = new RecipeViewModel(recipe);

            BackCommand = new NavigateCommand<CreateRecipeViewModel>(navigationService);
        }
    }
}
