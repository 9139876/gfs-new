using System.ComponentModel.DataAnnotations;
using GFS.Broker.Api.Enums;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.Portfolio
{
    public class OperationResponseDto
    {
        public PortfolioOperationResultTypeEnum PortfolioOperationResult { get; init; }

        public string? ErrorMessage { get; init; }

        [Required] public PortfolioInfoDto PortfolioInfo { get; init; }
    }
}