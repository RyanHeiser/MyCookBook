using Microsoft.Win32;
using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using MyCookBook.WPF.Stores.RecipeStores;
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

        public ImportBookCommand(RecipeStoreBase<RecipeBook> bookStore)
        {
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
                RecipeBook? book = await JsonSerializer.DeserializeAsync<RecipeBook>(stream);
                if (book != null)
                    await _bookStore.Create(book);
            }
        }
    }
}
