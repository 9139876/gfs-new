using System;
using System.ComponentModel.DataAnnotations.Schema;
using GFS.EF.Entities;
using GFS.GrailCommon.Models;
using GFS.Portfolio.Api.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Portfolio.DAL.Entities
{
    public class OperationEntity : IGuidKeyEntity
    {
        public Guid Id { get; set; }

        public DateTime MomentUtc { get; set; }

        public OperationTypeEnum OperationType { get; set; }

        [Column(TypeName = "jsonb")]
        public AssetIdentifier? AssetIdentifier { get; set; }

        /// <summary>
        /// Может быть положительным (покупка) и отрицательным (продажа) числом
        /// </summary>
        public int? AssetLotsChange { get; set; }

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