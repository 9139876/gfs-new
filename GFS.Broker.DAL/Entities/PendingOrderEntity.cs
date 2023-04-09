using GFS.Broker.Api.Enums;
using GFS.EF.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GFS.Broker.DAL.Entities;

public class PendingOrderEntity : GuidKeyEntity
{
    public PendingOrderType OrderType { get; set; }

    /// <summary>
    /// Идентификатор актива
    /// </summary>
    public Guid AssetId { get; set; }
    
    /// <summary>
    /// Цена исполнения ордера
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Количество единиц актива
    /// </summary>
    public decimal AssetCount { get; set; }
    
    public Guid PortfolioId { get; set; }

    public PortfolioEntity? Portfolio { get; set; }
}

public class PendingOrderEntityConfiguration : IEntityTypeConfiguration<PendingOrderEntity>
{
    public void Configure(EntityTypeBuilder<PendingOrderEntity> builder)
    {
        builder.ToTable("PendingOrders");
        builder.HasKey(e => e.Id);

        builder.HasOne(e => e.Portfolio)
            .WithMany(e => e.PendingOrders)
            .HasForeignKey(e => e.PortfolioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}