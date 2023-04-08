using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Broker.DAL.Entities
{
    public class OperationEntity : GuidKeyEntity
    {
        [UtcDate]
        public DateTime MomentUtc { get; set; }

        public PortfolioOperationTypeEnum PortfolioOperationType { get; set; }

        public Guid? AssetId { get; set; }

        /// <summary>
        /// Может быть положительным (покупка) и отрицательным (продажа) числом
        /// </summary>
        public int? AssetUnitsCountChange { get; set; }

        public decimal? AssetDealPrice { get; set; }

        /// <summary>
        /// Может быть положительным (пополнение, продажа) и отрицательным (изъятие, покупка) числом
        /// </summary>
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