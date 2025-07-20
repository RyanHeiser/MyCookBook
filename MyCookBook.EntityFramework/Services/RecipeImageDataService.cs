using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{
    public class RecipeImageDataService : ChildDataService<RecipeImage>
    {
        public RecipeImageDataService(MyCookBookDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public override async Task<RecipeImage?> Duplicate(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeImage? duplicate = await context.Set<RecipeImage>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == Id);

                if (duplicate != null)
                {
                    duplicate.Id = Guid.NewGuid();
                    context.Add(duplicate);
                    await context.SaveChangesAsync();
                }

                return duplicate;
            }
        }
    }
}
