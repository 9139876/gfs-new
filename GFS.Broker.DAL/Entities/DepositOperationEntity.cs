using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Broker.DAL.Entities
{
    public class DepositOperationEntity : GuidKeyEntity
    {
        [UtcDate]
        public DateTime MomentUtc { get; set; }

        public PortfolioDepositOperationType OperationType { get; set; }
        
        /// <summary>
        /// Может быть положительным (пополнение) и отрицательным (изъятие) числом
        /// </summary>
        public decimal CashChange { get; set; }

        public Guid PortfolioId { get; set; }

        public PortfolioEntity? Portfolio { get; set; }
    }

    public class DepositOperationEntityConfiguration : IEntityTypeConfiguration<DepositOperationEntity>
    {
        public void Configure(EntityTypeBuilder<DepositOperationEntity> builder)
        {
            builder.ToTable("DealOperations");
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Portfolio)
                .WithMany(e => e.DepositOperations)
                .HasForeignKey(e => e.PortfolioId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}