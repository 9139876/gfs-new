using System.ComponentModel.DataAnnotations;
using GFS.Broker.Api.Enums;

#pragma warning disable CS8618

namespace GFS.Broker.Api.Models.Portfolio
{
    public class PortfolioOperationResponseDto
    {
        public PortfolioOperationResultType PortfolioOperationResult { get; init; }

        public string? ErrorMessage { get; init; }

        [Required]
        public PortfolioInfoDto PortfolioInfo { get; init; }

        private PortfolioOperationResponseDto()
        {
        }

        public static PortfolioOperationResponseDto GetSuccessResponse(PortfolioInfoDto portfolioInfo)
            => new() { PortfolioOperationResult = PortfolioOperationResultType.Success, PortfolioInfo = portfolioInfo};

        public static PortfolioOperationResponseDto GetFailResponse(string error)
            => new() { PortfolioOperationResult = PortfolioOperationResultType.Fail, ErrorMessage = error};
    }
}