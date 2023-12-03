// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GFS.Common.Models;

public class WebAppResponseContainer<TPayload>
{
    private WebAppResponseContainer()
    {
    }

    public bool IsSuccess { get; private init; }

    public string? ErrorMessage { get; private init; }

    public TPayload? Payload { get; private init; }

    public static WebAppResponseContainer<TPayload> GetSuccessResponse(TPayload payload)
        => new() { IsSuccess = true, Payload = payload };

    public static WebAppResponseContainer<TPayload> GetFailResponse(string errorMessage)
        => new() { IsSuccess = false, ErrorMessage = errorMessage };
}