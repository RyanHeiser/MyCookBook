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
    public class CreateCategoryViewModel : ViewModelBase
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

        public CreateCategoryViewModel(RecipeStoreBase<RecipeBook> recipeBookStore, RecipeStoreBase<RecipeCategory> categoryStore, INavigationService closeNavigationService) 
        {
            _recipeBookStore = recipeBookStore;
            _closeNavigationService = closeNavigationService;
            
            CancelCommand = new NavigateCommand(closeNavigationService);

            if (categoryStore.Current == null)
            {
                SubmitCommand = new CompositeCommand(new CreateCategoryCommand(this, recipeBookStore, categoryStore), new NavigateCommand(closeNavigationService));
            }
            else
            {
                Category = categoryStore.Current;
                SubmitCommand = new CompositeCommand(new UpdateCategoryCommand(this, recipeBookStore, categoryStore), new NavigateCommand(closeNavigationService));
                IsEditing = true;
            }
        }
    }
}
