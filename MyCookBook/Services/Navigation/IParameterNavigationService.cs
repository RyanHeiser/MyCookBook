using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.Services.Navigation
{
    public interface IParameterNavigationService
    {
        void ParameterInitialize(params object[] parameters);
    }
}
