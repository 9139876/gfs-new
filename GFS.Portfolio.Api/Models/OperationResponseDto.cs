using System.ComponentModel.DataAnnotations;
using GFS.Portfolio.Api.Enums;

#pragma warning disable CS8618

namespace GFS.Portfolio.Api.Models
{
    public class OperationResponseDto
    {
        public PortfolioOperationResultTypeEnum PortfolioOperationResult { get; init; }

        public string? ErrorMessage { get; init; }

        [Required] public PortfolioInfoDto PortfolioInfo { get; init; }
    }
}