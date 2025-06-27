using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class NavigationStore
    {
        private Stack<Func<ViewModelBase>> _previousViewModels;

        protected ViewModelBase? _currentViewModel;
        private readonly RecipeStore _recipeStore;

        public ViewModelBase? CurrentViewModel => _currentViewModel;


        public event Action? CurrentViewModelChanged;

        public NavigationStore(RecipeStore recipeStore)
        {
            _previousViewModels = new Stack<Func<ViewModelBase>>();
            _recipeStore = recipeStore;
        }

        /// <summary>
        /// Navigates to a view model.
        /// </summary>
        /// <param name="createVM">A factory function which returns the new view model.</param>
        public void Navigate(Func<ViewModelBase> createVM)
        {
            _currentViewModel?.Dispose();

            // Add the new view model factory to the stack of previous view models and invoke it to set current view model.
            _previousViewModels.Push(createVM);
            _currentViewModel = createVM();

            OnCurrentViewModelChanged();
        }

        /// <summary>
        /// Navigate to the previous view model.
        /// </summary>
        public void NavigatePrevious()
        {

            // Pop the current view model factory from the stack and continue to do so while the next factory returns a CreateRecipeViewModel
            do
            {
                _previousViewModels.Pop();
            } while (_previousViewModels.Peek() is Func<CreateRecipeViewModel> || _previousViewModels.Peek().GetMethodInfo().ReturnType == _currentViewModel?.GetType());

            _currentViewModel?.Dispose();
            _currentViewModel = _previousViewModels.Peek().Invoke();

            // Set the app-wide recipe store to null if traversing back to RecipeListingView
            if (_currentViewModel.GetType() == typeof(RecipeListingViewModel))
            {
                _recipeStore.CurrentRecipe = null;
            }
            // Set the app-wide recipe category store to null if traversing back to CategoryListingView
            else if (_currentViewModel.GetType() == typeof(CategoryListingViewModel))
            {
                _recipeStore.CurrentCategory = null;
            }

            OnCurrentViewModelChanged();
        }

        protected void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
