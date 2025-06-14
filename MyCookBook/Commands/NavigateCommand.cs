﻿using MyCookBook.Models;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
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

        public NavigateCommand(NavigationService<TViewModel> navigationService, RecipeStore recipeStore)
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
