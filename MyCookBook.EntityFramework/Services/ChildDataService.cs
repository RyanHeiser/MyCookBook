using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public abstract class ChildDataService<T> : DataService<T> where T : ChildDomainObject
    {
        public ChildDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
            
        }

        /// <summary>
        /// Gets all items from parent.
        /// </summary>
        /// <param name="parentId">The Id of the parent</param>
        /// <returns>All children of the parent</returns>
        public virtual async Task<IEnumerable<T>> GetAllFromParent(Guid parentId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().Where(c => c.ParentId == parentId).ToListAsync();
            }
        }

        /// <summary>
        /// Moves an item to another parent.
        /// </summary>
        /// <param name="Id">The Id of the item to move.</param>
        /// <param name="newParentId">The Id of the new parent.</param>
        /// <returns>True if moved successfully</returns>
        public async Task<bool> Move(Guid Id, Guid newParentId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == Id);

                if (entity != null)
                {
                    entity.ParentId = newParentId;
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Updates an item.
        /// </summary>
        /// <param name="Id">The Id of the entity to update.</param>
        /// <param name="updatedEntity">The updated version of the entity.</param>
        /// <returns>The updated entity.</returns>
        public override async Task<T> Update(Guid Id, T updatedEntity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                T? entity = await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == Id);

                if (entity != null)
                {
                    updatedEntity.Id = entity.Id;
                    updatedEntity.ParentId = entity.ParentId;
                    context.Set<T>().Update(updatedEntity);
                    await context.SaveChangesAsync();
                }
                return updatedEntity;
            }
        }
    }
}
