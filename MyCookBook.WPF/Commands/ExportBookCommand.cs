using Microsoft.Win32;
using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Commands
{
    public class ExportBookCommand : AsyncCommandBase
    {
        private RecipeBookExportDataService _exportDataService;
        private RecipeBook? _book;

        public ExportBookCommand(RecipeBookExportDataService exportDataService)
        {
            _exportDataService = exportDataService;
        }

        public ExportBookCommand(RecipeBookExportDataService exportDataService, RecipeBook book)
        {
            _exportDataService = exportDataService;
            _book = book;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_book == null)
            {
                if (parameter is RecipeBook book)
                {
                    _book = book;
                }
                else
                {
                    return;
                }
            }

            string jsonString = await _exportDataService.ExportBook(_book.Id);

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "JSON (*.json)|*json";
            fileDialog.DefaultExt = "json";
            fileDialog.AddExtension = true;
            fileDialog.FileName = _book.Name + ".json";

            if (fileDialog.ShowDialog() == true)
            {
                File.WriteAllText(fileDialog.FileName, jsonString);
            }
        }
    }
}
