using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Stores;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyCookBook.WPF.Stores.RecipeStores;

namespace MyCookBook.WPF.ViewModels
{
    public class MoveViewModel : ViewModelBase
    {
        MoveStore _moveStore;

        public string? Name => _moveStore.Current?.Name;

        public ICommand MoveCommand { get; }
        public ICommand CancelCommand { get; }

        public MoveViewModel(MoveStore moveStore, NavigationStore navigationStore, ChildRecipeStoreBase<Recipe, RecipeCategory> recipeStore,
            ChildRecipeStoreBase<RecipeCategory, RecipeBook> categoryStore, RecipeStoreBase<RecipeBook> bookStore)
        {
            _moveStore = moveStore;

            if (moveStore.Current is Recipe recipe)
            {
                MoveCommand = new MoveCommand<Recipe, RecipeCategory>(moveStore, navigationStore, recipeStore, categoryStore, recipe);
            }
            else if (moveStore.Current is RecipeCategory category)
            {
                 MoveCommand = new MoveCommand<RecipeCategory, RecipeBook>(moveStore, navigationStore, categoryStore, bookStore, category);
            }

                CancelCommand = new CancelMoveCommand(moveStore);

            _moveStore.MoveUpdated += OnMoveUpdated;
        }

        public override void Dispose()
        {
            _moveStore.MoveUpdated -= OnMoveUpdated;
            base.Dispose();
        }

        private void OnMoveUpdated()
        {
            OnPropertyChanged(nameof(Name));
        }
    }
}
