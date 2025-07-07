using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Exceptions;
using MyCookBook.Domain.Models;
using MyCookBook.WPF.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using MyCookBook.WPF.Stores.RecipeStores;

namespace MyCookBook.WPF.ViewModels
{
    public class RecipeDisplayViewModel : ViewModelBase
    {
		private RecipeViewModel? _recipeViewModel;
		public RecipeViewModel? RecipeViewModel
        {
			get
			{
				return _recipeViewModel;
			}
			set
			{
				_recipeViewModel = value;
				OnPropertyChanged(nameof(RecipeViewModel));
			}
		}

        private byte[]? _rawImageData;
        public byte[]? RawImageData
        {
            get { return _rawImageData; }
            set
            {
                if (value != _rawImageData)
                {
                    _rawImageData = value;
                    OnPropertyChanged(nameof(RawImageData));
                }
            }
        }

        public ICommand BackCommand { get; }
		public ICommand EditCommand { get; }
		public ICommand DeleteCommand { get; }

        public ICommand LoadImageCommand { get; }

        public RecipeDisplayViewModel(RecipeCategoryStore categoryStore, RecipeStore recipeStore, RecipeImageStore imageStore,
			INavigationService createRecipeNavigationService, INavigationService previousNavigationService)
        {
			Recipe = recipeStore.Current;
			Category = categoryStore.Current;


            BackCommand = new NavigateCommand(previousNavigationService);
			EditCommand = new NavigateCommand(createRecipeNavigationService);
			DeleteCommand = new CompositeCommand(new DeleteCommand<Recipe>(recipeStore), BackCommand);

            LoadImageCommand = new LoadCommand<RecipeImage>(this, imageStore);

            LoadImageCommand.Execute(null);

            RecipeViewModel = new RecipeViewModel(recipeStore.Current);
            RawImageData = imageStore.Items.ElementAt(0).RawImageData;
        }

    }
}
