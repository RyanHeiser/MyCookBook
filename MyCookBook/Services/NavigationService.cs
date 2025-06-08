using MyCookBook.Models;
using MyCookBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Services
{
    public class NavigationService
    {
        private readonly Func<Type, ViewModelBase> _viewModelFactory;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public event Action? CurrentViewModelChanged;

        public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            ViewModelBase vm = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentViewModel = vm;
        }

        public void NavigateToRecipeDisplay(Recipe recipe) 
        {
            RecipeDisplayViewModel vm = new RecipeDisplayViewModel(recipe, this);
            CurrentViewModel = vm;
        }
    }
}
