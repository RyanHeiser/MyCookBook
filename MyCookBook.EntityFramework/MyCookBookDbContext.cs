using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using MyCookBook.EntityFramework.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework
{
    public class MyCookBookDbContext : DbContext
    {

        public DbSet<RecipeCategoryDTO> Categories { get; set; }
        public DbSet<RecipeDTO> Recipes { get; set; }

        public MyCookBookDbContext(DbContextOptions options) : base(options) { }
    }
}
