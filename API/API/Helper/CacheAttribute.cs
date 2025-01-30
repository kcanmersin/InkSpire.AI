using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _durationInSeconds;

    public CacheAttribute(int durationInSeconds)
    {
        _durationInSeconds = durationInSeconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetService<ICacheService>();
        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

        var cachedData = await cacheService.GetAsync<string>(cacheKey);
        if (cachedData != null)
        {
            context.Result = new ContentResult
            {
                Content = cachedData,
                ContentType = "application/json",
                StatusCode = 200
            };
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var responseJson = JsonSerializer.Serialize(objectResult.Value);
            await cacheService.SetAsync(cacheKey, responseJson, TimeSpan.FromSeconds(_durationInSeconds));
        }
    }

    private string GenerateCacheKey(HttpRequest request)
    {
        return $"{request.Path}{request.QueryString}";
    }
}
