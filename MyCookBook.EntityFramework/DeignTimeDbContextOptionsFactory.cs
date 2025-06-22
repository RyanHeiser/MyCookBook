using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework
{
    public class DeignTimeDbContextOptionsFactory : IDesignTimeDbContextFactory<MyCookBookDbContext>
    {
        public MyCookBookDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<MyCookBookDbContext>();
            options.UseSqlite("Data Source=MyCookBook.db");

            return new MyCookBookDbContext(options.Options);
        }
    }
}
