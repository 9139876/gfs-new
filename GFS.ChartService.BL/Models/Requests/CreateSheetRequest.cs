using System.ComponentModel.DataAnnotations;
using GFS.Common.Attributes.Validation;
using GFS.GrailCommon.Enums;

namespace GFS.ChartService.BL.Models.Requests;

public class CreateSheetRequest
{
    [Required]
    public string? Name { get; init; }
    
    [Required]
    public Guid AssetId { get; init; }

    public TimeFrameEnum TimeFrame { get; init; }

    [PositiveNumber]
    public decimal KPrice { get; init; }

    [PositiveNumberAttribute]
    public int RightEmptySpace { get; init; }

    public DateTime StartDate { get; init; }
}