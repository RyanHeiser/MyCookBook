using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyCookBook.EntityFramework
{
    public class MyCookBookDbContextFactory : IDesignTimeDbContextFactory<MyCookBookDbContext>
    {
        public MyCookBookDbContext CreateDbContext(string[] args = null)
        {
            string dbFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MyCookBook");
            Directory.CreateDirectory(dbFolder); // Ensure the folder exists

            string dbPath = Path.Combine(dbFolder, "MyCookBook.db");

            var options = new DbContextOptionsBuilder<MyCookBookDbContext>();
            options.UseSqlite($"Data Source={dbPath}");

            return new MyCookBookDbContext(options.Options);
        }
    }
}
