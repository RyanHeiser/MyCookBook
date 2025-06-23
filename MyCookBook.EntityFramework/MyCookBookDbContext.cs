using Microsoft.EntityFrameworkCore;
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
        //public DbSet<RecipeDTO> Recipes { get; set; }

        public MyCookBookDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<RecipeCategoryDTO>().OwnsMany(
            //    c => c.Recipes, c =>
            //    {
            //        c.WithOwner().HasForeignKey("CategoryId");
            //        c.Property<Guid>("Id");
            //        c.HasKey("Id");
            //    }).Property(r => r.Id).ValueGeneratedNever();

            //modelBuilder.Entity<RecipeCategoryDTO>().Property(c => c.Id).ValueGeneratedNever();

            //modelBuilder.Entity<RecipeCategoryDTO>().Property(c => c.Id).ValueGeneratedOnAdd();


            base.OnModelCreating(modelBuilder);
        }
    }
}
