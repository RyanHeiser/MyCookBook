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
    public class CreateRecipeViewModel : ViewModelBase
    {
        public ICommand CancelCommand { get; }
        public Recipe Recipe { get; }

        public CreateRecipeViewModel(RecipeBook recipeBook, NavigationService navigationService)
        {
            // temp recipe
            Recipe = new Recipe("Shepherd's pie", 60, 6, new List<string> { "potatoe", "lamb", "veggies" }, new List<string> { "mash potatoes", "cook lamb", "cook veggies", "combine" });

            CancelCommand = new NavigateCommand<RecipeDisplayViewModel>(navigationService);
        }
    }
}
