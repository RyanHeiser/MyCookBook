using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public interface IDataService<T>
    {
        Task<T> Create(T entity);

        Task<T> Get(Guid id);

        Task<IEnumerable<T>> GetAll();

        Task<T> Update(Guid id, T entity);

        Task<bool> Delete(Guid id);
    }
}
