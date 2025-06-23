using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCookBook.EntityFramework.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class CategoryDataService : IDataService<RecipeCategoryDTO>
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public CategoryDataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RecipeCategoryDTO> Create(RecipeCategoryDTO entity)
        {
            using(MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<RecipeCategoryDTO> createdResult = await context.Set<RecipeCategoryDTO>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }


        public async Task<RecipeCategoryDTO> Get(Guid id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategoryDTO? entity = await context.Set<RecipeCategoryDTO>().Include(c => c.Recipes).FirstOrDefaultAsync(e => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<RecipeCategoryDTO>> GetAll()
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<RecipeCategoryDTO>? entities = await context.Set<RecipeCategoryDTO>().Include(c => c.Recipes).ToListAsync();
                return entities;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategoryDTO? entity = await context.Set<RecipeCategoryDTO>().FirstOrDefaultAsync(e => e.Id == id);

                if (entity != null)
                {
                    context.Set<RecipeCategoryDTO>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<RecipeCategoryDTO> Update(Guid id, RecipeCategoryDTO entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                var existingCategory = await context.Categories
                    .Include(c => c.Recipes)
                    .FirstOrDefaultAsync(c => c.Id == id);

                existingCategory.Recipes.Clear();
                existingCategory.Recipes.AddRange(entity.Recipes);

                if (existingCategory == null)
                    throw new InvalidOperationException("Category not found.");

                // Update scalar properties
                existingCategory.Name = entity.Name;

                // Remove recipes not in the new list
                var recipeIds = entity.Recipes.Select(r => r.Id).ToHashSet();
                var recipesToRemove = existingCategory.Recipes.Where(r => !recipeIds.Contains(r.Id)).ToList();
                foreach (var recipe in recipesToRemove)
                    existingCategory.Recipes.Remove(recipe);

                // Add or update recipes
                foreach (var recipe in entity.Recipes)
                {
                    var existingRecipe = existingCategory.Recipes.FirstOrDefault(r => r.Id == recipe.Id);
                    if (existingRecipe == null)
                    {
                        // Attach new recipe
                        context.Entry(recipe).State = EntityState.Added;
                        existingCategory.Recipes.Add(recipe);
                    }
                    else
                    {
                        // Update existing recipe properties as needed
                        existingRecipe.Name = recipe.Name;
                        existingRecipe.Minutes = recipe.Minutes;
                        existingRecipe.Servings = recipe.Servings;
                        existingRecipe.Ingredients = recipe.Ingredients;
                        existingRecipe.Directions = recipe.Directions;
                        context.Entry(existingRecipe).State = EntityState.Modified;
                    }
                }

                await context.SaveChangesAsync();
                return existingCategory;
            }
        }
    }
}
