using Microsoft.AspNetCore.Mvc;

namespace GFS.ChartService.WebApp.Controllers;

public abstract class BaseControllerWithClientIdentifier : ControllerBase
{
    private const string CLIENT_ID_HEADER_NAME = "ClientId";
    
    protected Guid GetClientId()
    {
        if (!Request.Headers.TryGetValue(CLIENT_ID_HEADER_NAME, out var headerValue))
            throw new InvalidOperationException($"Не передан обязательный заголовок {CLIENT_ID_HEADER_NAME}");
        
        if(!Guid.TryParse(headerValue, out var clientId))
            throw new InvalidOperationException($"{CLIENT_ID_HEADER_NAME} должен иметь тип {nameof(Guid)}");

        return clientId;
    }
}