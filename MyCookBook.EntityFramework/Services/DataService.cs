using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public abstract class DataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public DataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public abstract Task<T?> Duplicate(Guid Id);

        /// <summary>
        /// Checks if the entity exists in the table.
        /// </summary>
        /// <param name="Id">The Id of the entity.</param>
        /// <returns>True if the entity is present.</returns>
        public async Task<bool> Contains(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().AnyAsync(e => e.Id == Id);
            }
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity.</returns>
        public async Task<T> Create(T entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Set<T>().AddAsync(entity);

                await context.SaveChangesAsync();
                return entity;
            }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="Id">The Id of the entity to delete.</param>
        /// <returns>True if successfully deleted.</returns>
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

        /// <summary>
        /// Gets an entity by Id.
        /// </summary>
        /// <param name="Id">The Id of the entity.</param>
        /// <returns>The entity</returns>
        public virtual async Task<T?> Get(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == Id);
                return entity;
            }
        }

        /// <summary>
        /// Gets all entities in the table.
        /// </summary>
        /// <returns>An IEnumerable of the entities.</returns>
        public virtual async Task<IEnumerable<T>> GetAll()
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T>? entities = await context.Set<T>().ToListAsync();
                return entities;
            }
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="Id">The Id of the entity to update.</param>
        /// <param name="updatedEntity">The updated version of the entity.</param>
        /// <returns>The updated entity.</returns>
        public virtual async Task<T> Update(Guid Id, T updatedEntity)
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
