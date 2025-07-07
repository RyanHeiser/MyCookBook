using MyCookBook.Domain.Models;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class LoadCommand<T> : AsyncCommandBase where T : DomainObject
    {
        private readonly ViewModelBase _viewModel;
        private readonly RecipeStoreBase<T> _store;

        public LoadCommand(ViewModelBase viewModel, RecipeStoreBase<T> store)
        {
            _viewModel = viewModel;
            _store = store;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _viewModel.IsLoading = true;

            try
            {
                await _store.Load();
                _viewModel.Update();
            }
            catch (Exception)
            {

            }

            _viewModel.IsLoading = false;
        }
    }

}
