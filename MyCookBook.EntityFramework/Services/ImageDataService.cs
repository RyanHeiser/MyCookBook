using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class ImageDataService : IDataService<RecipeImage>
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public ImageDataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RecipeImage> Create(RecipeImage entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Images.AddAsync(entity);

                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(Guid RecipeId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeImage? entity = await context.Images.FirstOrDefaultAsync(e => e.RecipeId == RecipeId);

                if (entity != null)
                {
                    context.Images.Remove(entity);

                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<RecipeImage> Get(Guid RecipeId)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeImage? entity = await context.Images.FirstOrDefaultAsync(e => e.RecipeId == RecipeId);
                return entity;
            }
        }

        public async Task<IEnumerable<RecipeImage>> GetAll()
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<RecipeImage>? entities = await context.Images.ToListAsync();
                return entities;
            }
        }

        public async Task<RecipeImage> Update(Guid RecipeId, RecipeImage entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeImage? image = await context.Images.AsNoTracking().FirstOrDefaultAsync(i => i.RecipeId == RecipeId);

                if (image != null)
                {
                    entity.RecipeId = image.RecipeId;
                    context.Images.Update(entity);
                    await context.SaveChangesAsync();
                }
                return entity;
            }
        }
    }
}
