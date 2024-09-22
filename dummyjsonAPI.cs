using System.Text.Json;
using FluentAssertions;
using Microsoft.Playwright;

namespace PlaywrightAPIDemo;

public class dummyjsonAPI : IClassFixture<PlaywrightDriver>
{
    private PlaywrightDriver _playwrightDriver;

    public dummyjsonAPI(PlaywrightDriver playwrightDriver)
    {
        _playwrightDriver = playwrightDriver;
    }
    [Fact]
    public async Task AuthenticateTest()
    {

        var response = await _playwrightDriver.ApiRequestContext?.PostAsync(url: "auth/login", new APIRequestContextOptions
        {
            DataObject = new
            {
                username = "emilys",
                password = "emilyspass",
                expiresInMins = 30
            }
        })!;

        var jsonString = await response.JsonAsync();
        // var token = jsonString.Value.GetProperty("accessToken").ToString();
        var autheticationResponse = jsonString?.Deserialize<Autheticate>();

        autheticationResponse?.accessToken.Should().NotBe(string.Empty);
        // token.Should().NotBe(string.Empty);

    }

    [Fact]
    public async Task GetDetail()
    {
        var token = await GetToken();
        var response = await _playwrightDriver.ApiRequestContext?.GetAsync(url: "auth/me", new APIRequestContextOptions()
        {
            Headers = new Dictionary<string, string>
            {
                {"Authorization", $"Bearer {token}"}
            }
        })!;

        var data = await response.JsonAsync();
        var name = data?.Deserialize<Name>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        name?.firstName.Should().Be("Emily");

    }

    private async Task<string?> GetToken()
    {


        var response = await _playwrightDriver.ApiRequestContext?.PostAsync(url: "auth/login", new APIRequestContextOptions
        {
            DataObject = new
            {
                username = "emilys",
                password = "emilyspass",
                expiresInMins = 30
            }
        })!;

        var jsonString = await response.JsonAsync();
        return jsonString?.Deserialize<Autheticate>()?.accessToken;
    }

    private record Autheticate(string accessToken) { }
    private record Name(string firstName) { }


}