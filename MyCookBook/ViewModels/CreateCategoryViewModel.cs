using MyCookBook.Commands;
using MyCookBook.Services.Navigation;
using MyCookBook.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.ViewModels
{
    public class CreateCategoryViewModel : ViewModelBase
    {
        private readonly RecipeBookStore _recipeBookStore;
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

        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public CreateCategoryViewModel(RecipeBookStore recipeBookStore, INavigationService closeNavigationService) 
        {
            _recipeBookStore = recipeBookStore;
            _closeNavigationService = closeNavigationService;

            AddCommand = new CompositeCommand(new CreateCategoryCommand(this, recipeBookStore), new NavigateCommand(closeNavigationService));
            CancelCommand = new NavigateCommand(closeNavigationService);
        }
    }
}
