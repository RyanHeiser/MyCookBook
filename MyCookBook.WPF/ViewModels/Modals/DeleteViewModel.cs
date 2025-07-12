using MyCookBook.Domain.Models;
using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyCookBook.WPF.ViewModels.Modals
{
    public class DeleteViewModel : ViewModelBase
    {
		private string _message;
		public string Message
		{
			get
			{
				return _message;
			}
			set
			{
				_message = value;
				OnPropertyChanged(nameof(Message));
			}
		}

		public DomainObject? ItemToDelete { get; }

		public ICommand DeleteCommand { get; }
		public ICommand CancelCommand { get; }

        public DeleteViewModel(DeleteStore deleteStore, string itemName, INavigationService closeNavigationService, ICommand deleteCommand)
        {
            _message = "Are you sure you want to delete this " + itemName + "?";

			DeleteCommand = new CompositeCommand(deleteCommand, new NavigateCommand(closeNavigationService));
			CancelCommand = new NavigateCommand(closeNavigationService);

            ItemToDelete = deleteStore.ItemToDelete;
        }

        public DeleteViewModel(DeleteStore deleteStore, string itemName, INavigationService closeNavigationService, ICommand deleteCommand, ICommand commands)
        {
            _message = "Are you sure you want to delete this " + itemName + "?";

            DeleteCommand = new CompositeCommand(deleteCommand, commands, new NavigateCommand(closeNavigationService));
            CancelCommand = new NavigateCommand(closeNavigationService);

            ItemToDelete = deleteStore.ItemToDelete;
        }
    }
}
