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

        public virtual async Task<IEnumerable<T>> GetAllFromParent(Guid parentId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().Where(c => c.ParentId == parentId).ToListAsync();
            }
        }

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
