using Microsoft.Playwright;

namespace PlaywrightAPIDemo;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var playwright = await Playwright.CreateAsync();
        var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions{
            BaseURL = "https://reqres.in/",
            IgnoreHTTPSErrors = true 
        });

        var response = await  requestContext.GetAsync(url: "api/users/2");
        var data = await    response.JsonAsync();
        System.Console.WriteLine(data);

    }
}