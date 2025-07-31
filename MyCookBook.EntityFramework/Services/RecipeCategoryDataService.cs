using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeCategoryDataService : ChildDataService<RecipeCategory>
    {
        public RecipeCategoryDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        /// <summary>
        /// Duplicates the RecipeCategory
        /// </summary>
        /// <param name="Id">The Id of the RecipeCategory to duplicate.</param>
        /// <returns>The duplicated RecipeCategory.</returns>
        public override async Task<RecipeCategory?> Duplicate(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeCategory? duplicate = await context.Set<RecipeCategory>()
                    .AsNoTracking()
                    .Include(c => c.Recipes)
                    .ThenInclude(r => r.Image)
                    .FirstOrDefaultAsync(c => c.Id == Id);

                if (duplicate != null)
                {
                    duplicate.RecipeCount = duplicate.Recipes.Count();
                    duplicate.Id = Guid.NewGuid();

                    foreach (Recipe recipe in duplicate.Recipes)
                    {
                        recipe.Id = Guid.NewGuid();

                        if (recipe.Image != null)
                            recipe.Image.Id = Guid.NewGuid();
                    }
                    context.Add(duplicate);
                    await context.SaveChangesAsync();
                }
                return duplicate;
            }
        }

        public override async Task<RecipeCategory?> Get(Guid Id)
        {
            RecipeCategory? category = await base.Get(Id);

            if (category == null)
                return null;

            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Recipe> recipes = await context.Set<Recipe>().Where(r => r.ParentId == Id).ToListAsync();
                category.ClearRecipes();
                foreach (Recipe recipe in recipes)
                {
                    category.AddRecipe(recipe);
                }

                return category;
            }
        }

        public override async Task<IEnumerable<RecipeCategory>> GetAll()
        {
            IEnumerable<RecipeCategory> categories = await base.GetAll();

            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                foreach (RecipeCategory category in categories)
                {
                    IEnumerable<Recipe> recipes = await context.Set<Recipe>().Where(r => r.ParentId == category.Id).ToListAsync();
                    category.ClearRecipes();
                    foreach (Recipe recipe in recipes)
                    {
                        category.AddRecipe(recipe);
                    } 
                }

                return categories;
            }
        }

        public override async Task<IEnumerable<RecipeCategory>> GetAllFromParent(Guid parentId)
        {
            IEnumerable<RecipeCategory> categories = await base.GetAllFromParent(parentId);

            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                foreach (RecipeCategory category in categories)
                {
                    IEnumerable<Recipe> recipes = await context.Set<Recipe>().Where(r => r.ParentId == category.Id).ToListAsync();
                    category.ClearRecipes();
                    foreach (Recipe recipe in recipes)
                    {
                        category.AddRecipe(recipe);
                    }
                }

                return categories;
            }
        }
    }
}
