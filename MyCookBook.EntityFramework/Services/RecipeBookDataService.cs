using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeBookDataService : DataService<RecipeBook>
    {
        public RecipeBookDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public override async Task<RecipeBook?> Duplicate(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeBook? duplicate = await context.Set<RecipeBook>()
                    .AsNoTracking()
                    .Include(b => b.Categories)
                    .ThenInclude(c => c.Recipes)
                    .ThenInclude(r => r.Image)
                    .FirstOrDefaultAsync(b => b.Id == Id);

                if (duplicate != null)
                {
                    duplicate.Id = Guid.NewGuid();
                    foreach (RecipeCategory category in duplicate.Categories)
                    {
                        category.Id = Guid.NewGuid();
                        foreach (Recipe recipe in category.Recipes)
                        {
                            recipe.Id = Guid.NewGuid();

                            if (recipe.Image != null)
                                recipe.Image.Id = Guid.NewGuid();
                        }
                    }
                    context.Add(duplicate);
                    await context.SaveChangesAsync();
                }
                return duplicate;
            }
        }
    }
}
