using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public interface IDataService<T> where T : DomainObject
    {
        Task<bool> Contains(Guid Id);

        Task<T> Create(T entity);

        Task<T?> Get(Guid Id);

        Task<IEnumerable<T>> GetAll();

        Task<T> Update(Guid Id, T entity);

        Task<bool> Delete(Guid Id);

        Task<T?> Duplicate(Guid Id);
    }
}
