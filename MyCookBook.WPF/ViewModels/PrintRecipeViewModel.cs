﻿using MyCookBook.Domain.Models;
using MyCookBook.WPF.Commands;
using MyCookBook.WPF.Services.Navigation;
using MyCookBook.WPF.Stores.RecipeStores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace MyCookBook.WPF.ViewModels
{
    public class PrintRecipeViewModel : ViewModelBase
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

        public FlowDocument? RecipeFlowDocument { get; }


        public ICommand BackCommand { get; }
        public ICommand PrintCommand { get; }

        public ICommand LoadImageCommand { get; }

        public PrintRecipeViewModel(RecipeStoreBase<Recipe> recipeStore, RecipeStoreBase<RecipeImage> imageStore, INavigationService previousNavigationService)
        {
            BackCommand = new NavigateCommand(previousNavigationService);
            PrintCommand = new PrintCommand();

            LoadImageCommand = new LoadCommand<RecipeImage>(this, imageStore);
            LoadImageCommand.Execute(null);

            RecipeViewModel = new RecipeViewModel(recipeStore.Current);
            RawImageData = imageStore.Items.ElementAt(0).RawImageData;

            RecipeFlowDocument = Application.LoadComponent(new Uri("../Resources/Documents/RecipeFlowDocument.xaml", UriKind.Relative)) as FlowDocument;
            if (RecipeFlowDocument != null)
            {
                RecipeFlowDocument.DataContext = this;
                RecipeFlowDocument.ColumnWidth = double.MaxValue; // set column width to max double value to prevent multiple columns
            }
        }
    }
}
