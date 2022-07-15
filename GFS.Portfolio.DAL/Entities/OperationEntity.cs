using System;
using GFS.EF.Entities;
using GFS.Portfolio.Api.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Portfolio.DAL.Entities
{
    public class OperationEntity: IGuidKeyEntity
    {
        public Guid Id { get; set; }
        public DateTime MomentUtc { get; set; }
        public OperationTypeEnum OperationType { get; set; }
        public Guid? AssetId { get; set; }
        public int? AssetsChangeCount { get; set; }
        public decimal? AssetDealPrice { get; set; }
        public decimal CashChange { get; set; }
        
        public Guid PortfolioId { get; set; }
        
        public PortfolioEntity? Portfolio { get; set; }
    }

    public class OperationEntityConfiguration : IEntityTypeConfiguration<OperationEntity>
    {
        public void Configure(EntityTypeBuilder<OperationEntity> builder)
        {
            builder.ToTable("Operations");
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.Portfolio)
                .WithMany(e => e.Operations)
                .HasForeignKey(e => e.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}