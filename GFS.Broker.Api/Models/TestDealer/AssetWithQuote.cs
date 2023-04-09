using System.ComponentModel.DataAnnotations;
using GFS.GrailCommon.Models;
#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.TestDealer;

public class AssetWithQuote
{
    [Required]
    public Guid AssetId { get; init; }
    
    [Required]
    public QuoteModel Quote { get; init; }
}