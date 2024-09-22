using Microsoft.Playwright;

namespace PlaywrightAPIDemo;

public class PlaywrightDriver : IDisposable
{
    private readonly Task<IAPIRequestContext> _requestContext;
    public PlaywrightDriver()
    {
        _requestContext = CreateApiContext();
    }

    public IAPIRequestContext? ApiRequestContext => _requestContext.GetAwaiter().GetResult();
    private async Task<IAPIRequestContext> CreateApiContext()
    {
        var playwright = await Playwright.CreateAsync();
        return await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = "https://dummyjson.com/",
            IgnoreHTTPSErrors = true
        });
    }
    public void Dispose()
    {
        _requestContext.Dispose();
    }
}

