using System.ComponentModel.DataAnnotations;
using GFS.Broker.Api.Enums;
using GFS.Common.Attributes.Validation;

namespace GFS.Broker.BL.Models;

public class PerformDealOperationRequestDto
{
    protected PerformDealOperationRequestDto()
    {
    }

    public PerformDealOperationRequestDto(
        Guid portfolioId, DealOperationType operationType, OperationPerformType performType, DateTime momentUtc, Guid assetId, decimal dealPrice, decimal deltaCash, int deltaAsset)
    {
        PortfolioId = portfolioId;
        OperationType = operationType;
        PerformType = performType;
        MomentUtc = momentUtc;
        AssetId = assetId;
        DealPrice = dealPrice;
        DeltaCash = deltaCash;
        DeltaAsset = deltaAsset;
    }

    [Required]
    public Guid PortfolioId { get; }

    [Required]
    public DealOperationType OperationType { get; }

    [Required]
    public OperationPerformType PerformType { get; }

    [Required]
    [UtcDate]
    public DateTime MomentUtc { get; }

    [Required]
    public Guid AssetId { get; }

    [Required]
    public decimal DealPrice { get; }

    /// <summary>
    /// Изменение суммы на счёте после сделки
    /// </summary>
    [Required]
    public decimal DeltaCash { get; }

    /// <summary>
    /// Изменение количества единиц инструмента в портфеле после сделки
    /// </summary>
    [Required]
    public int DeltaAsset { get; }
}