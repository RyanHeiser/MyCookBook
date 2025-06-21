using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.ViewModels
{
    public class StringViewModel : ViewModelBase
    {
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public StringViewModel(string str)
        {
            Text = str;
        }
    }
}
