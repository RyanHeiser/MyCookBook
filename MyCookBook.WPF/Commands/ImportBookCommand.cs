using Microsoft.Win32;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class ImportBookCommand : AsyncCommandBase
    {
        RecipeBookIODataService _importDataService;

        public ImportBookCommand(RecipeBookIODataService importDataService)
        {
            _importDataService = importDataService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "JSON (*.json)|*json";
            fileDialog.AddExtension = true;

            if (fileDialog.ShowDialog() == true)
            {
                await _importDataService.ImportBook(File.ReadAllText(fileDialog.FileName));
            }
        }
    }
}
