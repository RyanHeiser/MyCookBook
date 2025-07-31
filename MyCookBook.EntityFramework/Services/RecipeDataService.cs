using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeDataService : ChildDataService<Recipe>
    {
        public RecipeDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        /// <summary>
        /// Duplicates the Recipe
        /// </summary>
        /// <param name="Id">The Id of the Recipe to duplicate.</param>
        /// <returns>The duplicated Recipe.</returns>
        public override async Task<Recipe?> Duplicate(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                Recipe? duplicate = await context.Set<Recipe>()
                    .AsNoTracking()
                    .Include(r => r.Image)
                    .FirstOrDefaultAsync(r => r.Id == Id);

                if (duplicate != null)
                {
                    duplicate.Id = Guid.NewGuid();

                    if (duplicate.Image != null)
                        duplicate.Image.Id = Guid.NewGuid();

                    context.Add(duplicate);
                    await context.SaveChangesAsync();
                }

                return duplicate;
            }
        }
    }
}
