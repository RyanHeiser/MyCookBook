using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.ViewModels
{
    public class RecipeBookViewModel : ViewModelBase
    {
        public RecipeBook Book { get; set; }
        public string Name => Book.Name;

        public RecipeBookViewModel(RecipeBook book)
        {
            Book = book;
        }
    }
}
