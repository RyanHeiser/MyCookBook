using MyCookBook.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Exceptions
{
    public class InvalidParametersException<TViewModel> : Exception where TViewModel : ViewModelBase
    {
        public object[] Parameters { get; }
        public Type ViewModel { get; }

        public InvalidParametersException(object[] parameters) {
            Parameters = parameters;
            ViewModel = typeof(TViewModel);
        }
    }
}
