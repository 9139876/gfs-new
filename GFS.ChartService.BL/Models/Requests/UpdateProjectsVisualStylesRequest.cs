using System.ComponentModel.DataAnnotations;
using GFS.ChartService.BL.Models.ProjectViewModel.Sheet.Layers;
#pragma warning disable CS8618

namespace GFS.ChartService.BL.Models.Requests;

public class UpdateProjectsVisualStylesRequest
{
    [Required]
    public GridLayerProperties GridLayerProperties { get; init; }
}