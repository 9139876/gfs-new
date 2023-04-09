using System.ComponentModel.DataAnnotations;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#pragma warning disable CS8618

namespace GFS.Broker.DAL.Entities
{
    public class PortfolioEntity : GuidKeyEntity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<DealOperationEntity> DealOperations { get; set; } = new();
        
        public List<DepositOperationEntity> DepositOperations { get; set; } = new();
        
        public List<PendingOrderEntity> PendingOrders { get; set; } = new();
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