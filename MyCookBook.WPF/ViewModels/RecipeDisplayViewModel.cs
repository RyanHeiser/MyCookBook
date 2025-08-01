﻿using MyCookBook.WPF.Commands;
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
using MyCookBook.WPF.Stores;

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

        public ICommand PrintCommand { get; }
        public ICommand MoveCommand {  get; }
		public ICommand EditCommand { get; }
		public ICommand DeleteCommand { get; }


        public ICommand LoadImageCommand { get; }

        public RecipeDisplayViewModel(RecipeStoreBase<RecipeCategory> categoryStore, RecipeStoreBase<Recipe> recipeStore, 
            RecipeStoreBase<RecipeImage> imageStore, DeleteStore deleteStore, MoveStore moveCopyStore, INavigationService printRecipeNavigationService,
			INavigationService createRecipeNavigationService, INavigationService deleteNavigationService, INavigationService previousNavigationService)
        {
			Recipe = recipeStore.Current;
			Category = categoryStore.Current;


            BackCommand = new NavigateCommand(previousNavigationService);

            PrintCommand = new NavigateCommand(printRecipeNavigationService);
            MoveCommand = new StartMoveCommand<Recipe>(moveCopyStore, recipeStore.Current);
			EditCommand = new NavigateCommand(createRecipeNavigationService);
			DeleteCommand = new CompositeCommand(new SetDeleteStoreCommand(deleteStore), new NavigateCommand(deleteNavigationService));


            LoadImageCommand = new LoadCommand<RecipeImage>(this, imageStore);
            LoadImageCommand.Execute(null);

            RecipeViewModel = new RecipeViewModel(recipeStore.Current);
            if (imageStore.Items.Count() > 0)
                RawImageData = imageStore.Items.ElementAt(0).RawImageData;
        }

    }
}
