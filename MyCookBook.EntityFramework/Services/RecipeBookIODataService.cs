using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework.Services
{

    public class RecipeBookIODataService
    {
        private readonly MyCookBookDbContextFactory _contextFactory;

        public RecipeBookIODataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> ExportBook(Guid Id)
        {
            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                RecipeBook? book = await context.RecipeBooks
                                                .Include(b => b.Categories)
                                                .ThenInclude(c => c.Recipes)
                                                .ThenInclude(r => r.Image)
                                                .FirstOrDefaultAsync(b => b.Id == Id);

                return JsonSerializer.Serialize(book);
            }
        }

        public async Task<bool> ImportBook(string bookJson)
        {
            RecipeBook? book = JsonSerializer.Deserialize<RecipeBook>(bookJson);

            if (book == null)
            {
                return false;
            }

            using (MyCookBookDbContext context = _contextFactory.CreateDbContext())
            {
                if (context.RecipeBooks.Any(b => b.Id == book.Id))
                {
                    return false;
                }

                await context.RecipeBooks.AddAsync(book);
                await context.SaveChangesAsync();

                return true;
            }

        }
    }
}
