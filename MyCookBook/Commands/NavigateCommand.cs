using MyCookBook.Models;
using MyCookBook.Services.Navigation;
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
        private readonly NavigationService<TViewModel> _navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Navigates to the view model.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                try
                {
                    object[] parameters = parameter as object[];
                    _navigationService.Navigate(parameters);
                }
                catch (Exception ex)
                {

                }
            } 
            else
            {
                _navigationService.Navigate();
            }
        }
    }
}
