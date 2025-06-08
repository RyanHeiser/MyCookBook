using MyCookBook.Models;
using MyCookBook.Services;
using MyCookBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Navigates to the view model.
        /// </summary>
        /// <param name="parameter">A recipe if navigating to the RecipeDisplayViewModel</param>
        public override void Execute(object? parameter)
        {
            if (typeof(TViewModel) == typeof(RecipeDisplayViewModel) && parameter is Recipe recipe)
            {
                recipe = (Recipe) parameter;
                _navigationService.NavigateToRecipeDisplay(recipe);
            } 
            else
            {
                _navigationService.NavigateTo<TViewModel>();
            }    
        }
    }
}
