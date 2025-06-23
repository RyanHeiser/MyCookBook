using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class CategoryDataService : IDataService<RecipeCategory>
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public CategoryDataService(MyCookBookDbContextFactory contextFactory, RecipeBook recipeBook)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RecipeCategory> Create(RecipeCategory entity)
        {
            using(MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<RecipeCategory> createdResult = await context.Set<RecipeCategory>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }


        public async Task<RecipeCategory> Get(Guid id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? entity = await context.Set<RecipeCategory>().Include(c => c.Recipes).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<RecipeCategory>> GetAll()
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<RecipeCategory>? entities = await context.Set<RecipeCategory>().Include(c => c.Recipes).ToListAsync();
                return entities;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? entity = await context.Set<RecipeCategory>().FirstOrDefaultAsync(e => e.Id == id);

                if (entity != null)
                {
                    context.Set<RecipeCategory>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<RecipeCategory> Update(Guid id, RecipeCategory entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? existing = await Get(id);

                HashSet<Guid> entityIds = entity.Recipes.Select(r => r.Id).ToHashSet();
                List<Recipe> recipesToRemove = existing.Recipes.Where(r => !entityIds.Contains(r.Id)).ToList();

                entity.Id = id;

                foreach (Recipe recipe in entity.Recipes)
                {
                    if (recipesToRemove.Contains(recipe))
                    {
                        context.Set<Recipe>().Remove(recipe);
                    } 
                    else if (!context.Set<Recipe>().Any(r => r.Id == recipe.Id))
                    {
                        context.Set<Recipe>().Add(recipe);
                    }
                }

                context.Set<RecipeCategory>().Update(entity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
