using MyCookBook.Domain.Models;
using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels.Modals
{
    public class CreateRecipeBookViewModel : ViewModelBase
    {
        private readonly RecipeStoreBase<RecipeBook> _recipeBookStore;
        private readonly INavigationService _closeNavigationService;

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get
            {
                return _isEditing;
            }
            private set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateRecipeBookViewModel(RecipeStoreBase<RecipeBook> recipeBookStore, RecipeStoreBase<RecipeCategory> categoryStore, INavigationService closeNavigationService)
        {
            _recipeBookStore = recipeBookStore;
            _closeNavigationService = closeNavigationService;

            CancelCommand = new NavigateCommand(closeNavigationService);

            if (recipeBookStore.Current == null)
            {
                SubmitCommand = new CreateRecipeBookCommand(this, closeNavigationService, recipeBookStore);
            }
            else
            {
                SubmitCommand = new UpdateRecipeBookCommand(this, closeNavigationService, recipeBookStore);
                IsEditing = true;
            }
        }
    }
}
