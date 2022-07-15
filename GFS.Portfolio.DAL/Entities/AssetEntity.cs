using System;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Portfolio.DAL.Entities
{
    public class AssetEntity : IGuidKeyEntity
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public int Count { get; set; }
        public Guid PortfolioId { get; set; }

        public PortfolioEntity? Portfolio { get; set; }
    }

    public class AssetEntityConfiguration : IEntityTypeConfiguration<AssetEntity>
    {
        public void Configure(EntityTypeBuilder<AssetEntity> builder)
        {
            builder.ToTable("Assets");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Portfolio)
                .WithMany(e => e.Assets)
                .HasForeignKey(e => e.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}