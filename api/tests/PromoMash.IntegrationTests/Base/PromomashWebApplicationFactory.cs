using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Promomash.Application.Features.Users.LoginUser;
using Promomash.EntityFramework.Initialization.Seed;

namespace PromoMash.IntegrationTests.Base;

public class PromomashWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("It");
    }

    public HttpClient GetClient()
    {
        return CreateClient();
    }

    public async Task<HttpClient> GetAuthencticatedClient()
    {
        var token = await GetJwtTokenAsync();
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private async Task<string> GetJwtTokenAsync()
    {
        var formData = new MultipartFormDataContent
        {
            { new StringContent(SeedData.Admin.Login), "username" },
            { new StringContent(SeedData.Admin.PasswordHash), "password" }
        };
        var response = await GetClient()
            .PostAsync($"/api/v1/auth/login", formData);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var loginUserCommandResponse =
            System.Text.Json.JsonSerializer.Deserialize<LoginUserCommandResponse>(jsonResponse);

        return loginUserCommandResponse?.Token ?? string.Empty;
    }
}