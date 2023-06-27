using System;
using Microsoft.EntityFrameworkCore;
using GameShop.Core.Entities;
using GameShop.Infrastructure.EntityFramework.DatabaseConfiguration;

namespace GameShop.Infrastructure.EntityFramework
{
    public class GameShopContext : DbContext
    {
        public GameShopContext(DbContextOptions<GameShopContext> options)
            : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Videogame> Videogames { get; set; }

        public DbSet<Rent> Rents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Category>(new CategoryEntityConfiguration());
            modelBuilder.ApplyConfiguration<Videogame>(new VideogameEntityConfiguration());
            modelBuilder.ApplyConfiguration<Rent>(new RentEntityConfiguration());
        }
    }
}