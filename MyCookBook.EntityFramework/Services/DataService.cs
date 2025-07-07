using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class DataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public DataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Set<T>().AddAsync(entity);

                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == Id);

                if (entity != null)
                {
                    context.Set<T>().Remove(entity);

                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<T> Get(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == Id);
                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T>? entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        public async Task<T> Update(Guid Id, T updatedEntity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);

                if (entity != null)
                {
                    updatedEntity.Id = entity.Id;
                    context.Set<T>().Update(updatedEntity);
                    await context.SaveChangesAsync();
                }
                return updatedEntity;
            }
        }
    }
}
