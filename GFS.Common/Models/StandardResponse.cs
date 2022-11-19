namespace GFS.Common.Models;

public class StandardResponse
{
    private StandardResponse()
    {
    }

    public bool IsSuccess { get; init; }

    public string? ErrorMessage { get; init; }

    public static StandardResponse GetSuccessResponse()
        => new StandardResponse { IsSuccess = true };

    public static StandardResponse GetFailResponse(string errorMessage)
        => new StandardResponse { IsSuccess = false, ErrorMessage = errorMessage};
}