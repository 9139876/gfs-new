using System;
using System.Collections.Generic;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Portfolio.DAL.Entities
{
    public class PortfolioEntity : IGuidKeyEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<OperationEntity> Operations { get; set; } = new();
    }

    public class PortfolioEntityConfiguration : IEntityTypeConfiguration<PortfolioEntity>
    {
        public void Configure(EntityTypeBuilder<PortfolioEntity> builder)
        {
            builder.ToTable("Portfolios");
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}