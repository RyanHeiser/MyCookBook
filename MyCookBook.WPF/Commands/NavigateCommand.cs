using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;

namespace MyCookBook.WPF.Commands
{
    public class NavigateCommand : CommandBase 
    {
        private readonly INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        /// <summary>
        /// Navigates to the view model.
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
