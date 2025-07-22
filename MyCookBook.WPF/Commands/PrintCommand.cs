using MyCookBook.WPF.Converters;
using MyCookBook.WPF.ViewModels;
using MyCookBook.WPF.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace MyCookBook.WPF.Commands
{
    public class PrintCommand : AsyncCommandBase
    {
        //RecipeDisplayViewModel _viewModel;

        //public PrintCommand(RecipeDisplayViewModel viewModel)
        //{
        //    _viewModel = viewModel;
        //}

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter is FlowDocument document)
            {
                DocumentPaginator paginator = ((IDocumentPaginatorSource) document).DocumentPaginator;
                PrintDialog dialog = new PrintDialog();
                if (dialog.ShowDialog() == true)
                {
                    dialog.PrintDocument(paginator, "Print recipe");
                }
            }
        }
    }
}
