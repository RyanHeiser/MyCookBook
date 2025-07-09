using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class ChildDataService<T> : DataService<T> where T : ChildDomainObject
    {
        public ChildDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
            
        }

        public async Task<IEnumerable<T>> GetAllFromParent(Guid parentId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().Where(c => c.ParentId == parentId).ToListAsync();
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
