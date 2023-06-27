using System;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GameShop.Core.Entities;

namespace GameShop.Infrastructure.EntityFramework.DatabaseConfiguration
{
    public class VideogameEntityConfiguration : IEntityTypeConfiguration<Videogame>
    {
        public void Configure(EntityTypeBuilder<Videogame> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Name)
                .IsRequired();
        }
    }
}
