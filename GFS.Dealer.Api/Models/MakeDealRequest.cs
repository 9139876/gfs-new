using GFS.FakeDealer.Api.Enums;
using GFS.GrailCommon.Enums;

namespace GFS.FakeDealer.Api.Models;

public class MakeDealRequest
{
    public string FIGI { get; init; }
    
    public int UnitsCount { get; init; }
 
    public DateTime DealDateUtc { get; init; } = DateTime.UtcNow;
    
    public TimeFrameEnum TimeFrame { get; init; }
    
    public DealerOperationTypeEnum OperationType { get; init; }
}