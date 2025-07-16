using Microsoft.Win32;
using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using MyCookBook.WPF.Stores.RecipeStores;
using MyCookBook.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class ImportBookCommand : AsyncCommandBase
    {
        readonly RecipeStoreBase<RecipeBook> _bookStore;
        readonly ViewModelBase _viewModel;

        public ImportBookCommand(ViewModelBase viewModel, RecipeStoreBase<RecipeBook> bookStore)
        {
            _viewModel = viewModel;
            _bookStore = bookStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "JSON (*.json)|*json";
            fileDialog.AddExtension = true;

            if (fileDialog.ShowDialog() == true)
            {
                using FileStream stream = File.OpenRead(fileDialog.FileName);
                RecipeBook? book;
                try
                {
                    book = await JsonSerializer.DeserializeAsync<RecipeBook>(stream);
                }
                catch (Exception)
                {
                    _viewModel.ErrorMessage = "Failed to import from " + fileDialog.FileName;
                    return;
                }

                if (book != null)
                {
                    bool created = await _bookStore.Create(book);
                    if (!created)
                    {
                        _viewModel.ErrorMessage = "Failed to import from " + fileDialog.FileName + ". This book may already exist.";
                    }
                }
            }
        }
    }
}
