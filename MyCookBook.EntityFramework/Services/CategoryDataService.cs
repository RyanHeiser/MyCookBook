﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class CategoryDataService : IDataService<RecipeCategory>
    {
        protected readonly MyCookBookDbContextFactory _contextFactory;

        public CategoryDataService(MyCookBookDbContextFactory contextFactory)
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

        public async Task<RecipeCategory> Get(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? entity = await context.Set<RecipeCategory>().Include(c => c.Recipes).FirstOrDefaultAsync(e => e.CategoryId == Id);
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

        public async Task<bool> Delete(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? entity = await context.Set<RecipeCategory>().FirstOrDefaultAsync(e => e.CategoryId == Id);

                if (entity != null)
                {
                    context.Set<RecipeCategory>().Remove(entity);
                    await context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<RecipeCategory> Update(Guid Id, RecipeCategory entity)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                //RecipeCategory? category = context.Categories
                //    //.Include(c => c.Recipes)
                //    .FirstOrDefault(c => c.CategoryId == Id);

                
                entity.CategoryId = Id;
                context.Categories.Update(entity);
                await context.SaveChangesAsync();
                
                return entity;
            }
        }
    }
}
