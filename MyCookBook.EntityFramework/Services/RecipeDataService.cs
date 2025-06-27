using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeDataService : IDataService<Recipe>
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public RecipeDataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Recipe> Create(Recipe entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Recipes.AddAsync(entity);

                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                Recipe? entity = await context.Recipes.FirstOrDefaultAsync(e => e.Id == Id);

                if (entity != null)
                {
                    context.Recipes.Remove(entity);

                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<Recipe> Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Recipe>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Recipe> Update(Guid Id, Recipe entity)
        {
            throw new NotImplementedException();
        }
    }
}
