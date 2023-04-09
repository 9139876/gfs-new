using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Broker.DAL.Entities
{
    public class DealOperationEntity : GuidKeyEntity
    {
        [UtcDate]
        public DateTime MomentUtc { get; set; }

        public DealOperationType OperationType { get; set; }

        public Guid AssetId { get; set; }

        /// <summary>
        /// Как выполнилась операция
        /// </summary>
        public OperationPerformType PerformType { get; set; }
        
        /// <summary>
        /// Может быть положительным (покупка) и отрицательным (продажа) числом
        /// </summary>
        public int AssetUnitsCountChange { get; set; }

        public decimal AssetDealPrice { get; set; }

        /// <summary>
        /// Может быть положительным (продажа) и отрицательным (покупка) числом
        /// </summary>
        public decimal CashChange { get; set; }

        public Guid PortfolioId { get; set; }

        public PortfolioEntity? Portfolio { get; set; }
    }

    public class DealOperationEntityConfiguration : IEntityTypeConfiguration<DealOperationEntity>
    {
        public void Configure(EntityTypeBuilder<DealOperationEntity> builder)
        {
            builder.ToTable("DealOperations");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Portfolio)
                .WithMany(e => e.DealOperations)
                .HasForeignKey(e => e.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}