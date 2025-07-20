using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class CopyCommand<TChild, TParent> : AsyncCommandBase where TChild : ChildDomainObject where TParent : DomainObject
    {
        MoveStore _moveStore;
        NavigationStore _navigationStore;
        ChildRecipeStoreBase<TChild, TParent> _childStore;
        RecipeStoreBase<TParent> _parentStore;
        TChild _itemToMove;

        public CopyCommand(MoveStore moveStore, NavigationStore navigationStore, ChildRecipeStoreBase<TChild, TParent> childStore, RecipeStoreBase<TParent> parentStore, TChild itemToMove)
        {
            _moveStore = moveStore;
            _navigationStore = navigationStore;
            _childStore = childStore;
            _parentStore = parentStore;
            _itemToMove = itemToMove;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            bool canExecute = _itemToMove is Recipe && _navigationStore.CurrentViewModel is RecipeListingViewModel ||
                              _itemToMove is RecipeCategory && _navigationStore.CurrentViewModel is CategoryListingViewModel;

            return canExecute && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_parentStore.Current != null)
            {
                await _childStore.Copy(_itemToMove, _parentStore.Current.Id);
                _moveStore.IsMoving = false;
                await _childStore.Load();
                _navigationStore.CurrentViewModel?.Update();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            OnCanExecuteChanged();
        }
    }
}
