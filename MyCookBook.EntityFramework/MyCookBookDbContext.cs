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
        public DbSet<RecipeBook> RecipeBooks { get; set; }
        public DbSet<RecipeCategory> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeImage> Images { get; set; }

        public MyCookBookDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeBook>()
                .HasMany(b => b.Categories)
                .WithOne()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeCategory>()
                .HasMany(c => c.Recipes)
                .WithOne()
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.Image)
                .WithOne()
                .HasForeignKey<RecipeImage>(i => i.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Recipe>(b =>
            //{
            //    b.Property(i => i.Id).IsRequired();
            //    b.HasOne<RecipeImage>()
            //        .WithOne()
            //        .HasForeignKey<RecipeImage>(i => i.ParentId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //});

            base.OnModelCreating(modelBuilder);
        }
    }
}
