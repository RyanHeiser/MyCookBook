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

    public class RecipeBookExportDataService
    {
        private readonly MyCookBookDbContextFactory _contextFactory;

        public RecipeBookExportDataService(MyCookBookDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Creates a JSON of a RecipeBook.
        /// </summary>
        /// <param name="Id">The Id of the RecipeBook.</param>
        /// <returns>A JSON string created from the RecipeBook.</returns>
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
    }
}
