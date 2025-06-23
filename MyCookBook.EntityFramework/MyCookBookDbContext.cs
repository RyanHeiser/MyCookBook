using Microsoft.EntityFrameworkCore;
using MyCookBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCookBook.EntityFramework
{
    public class MyCookBookDbContext : DbContext
    {

        public DbSet<RecipeCategory> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public MyCookBookDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeCategory>()
                .HasMany(c => c.Recipes)
                .WithOne()
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<RecipeCategoryDTO>().Property(c => c.Id).ValueGeneratedNever();

            //modelBuilder.Entity<RecipeCategoryDTO>().Property(c => c.Id).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
