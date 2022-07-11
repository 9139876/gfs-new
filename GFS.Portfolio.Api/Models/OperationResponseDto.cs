using GFS.Portfolio.Api.Enums;

namespace GFS.Portfolio.Api.Models
{
    public class OperationResponseDto
    {
        public OperationResultTypeEnum OperationResult { get; set; }
        public string? ErrorMessage { get; set; }
        public PortfolioInfoDto PortfolioInfo { get; set; }
    }
}