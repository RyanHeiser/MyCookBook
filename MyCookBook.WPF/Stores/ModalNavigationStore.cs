using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.WPF.Stores
{
    public class ModalNavigationStore : NavigationStore
    {
        public bool IsOpen => CurrentViewModel != null;

        public void Close()
        {
            CurrentViewModel = null;
        }
    }
}
