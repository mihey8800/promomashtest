using System.Net;
using System.Net.Http.Json;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Promomash.Application.Features.Countries.ChangeName;
using Promomash.Application.Features.Countries.CreateCountry;
using Promomash.Application.Features.Countries.GetCountries;
using Promomash.Application.Features.Countries.GetCountry;
using Promomash.Domain.Entities;
using Promomash.EntityFramework.Initialization.Seed;
using Promomash.Host;
using PromoMash.IntegrationTests.Base;
using Shouldly;

namespace PromoMash.IntegrationTests.Controllers;

public class CountryControllerTests : IClassFixture<PromomashWebApplicationFactory<Program>>
{
    private readonly PromomashWebApplicationFactory<Program> _factory;

    public CountryControllerTests(PromomashWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCountries_ReturnsSuccessResult()
    {
        var client = _factory.GetClient();

        var response = await client.GetAsync("/api/v1/country");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<GetCountriesQueryResponse>>(responseString);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetCountry_ReturnsSuccessResult()
    {
        var country = SeedData.Countries.First();
        await GetCountry_ReturnsSuccessResult_Inner(country);
    }

    [Fact]
    public async Task<Country> CreateCountry_ReturnsSuccessResult()
    {
        var client = await _factory.GetAuthencticatedClient();
        var rnd = new Random();
        var randomString = rnd.Next(1000, 10000);

        var createCountryCommand = new CreateCountryCommand
        {
           Name = $"Test Country-{randomString}"
        };

        var response = await client.PostAsJsonAsync("/api/v1/country", createCountryCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateCountryCommandResponse>(responseString);
        result.ShouldNotBeNull();

        var createCountryReturnsSuccessResult = createCountryCommand.Adapt<Country>();
        createCountryReturnsSuccessResult.Id = result.Id;

        await GetCountry_ReturnsSuccessResult_Inner(createCountryReturnsSuccessResult);

        return createCountryReturnsSuccessResult;
    }

    [Fact]
    public async Task CreateCountry_ReturnsBadRequestResult()
    {
        var client = await _factory.GetAuthencticatedClient();

        var createCountryCommand = new CreateCountryCommand();
        var response = await client.PostAsJsonAsync("/api/v1/country", createCountryCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ProblemDetails>(responseString);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task DeleteCountry_ReturnsBadRequestResult()
    {
        var countryReturnsSuccessResult = await CreateCountry_ReturnsSuccessResult();
        var client = await _factory.GetAuthencticatedClient();

        var response = await client.DeleteAsync($"/api/v1/country/{countryReturnsSuccessResult.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var getResponse = await client.GetAsync($"/api/v1/country/{countryReturnsSuccessResult.Id}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<GetCountryQueryResponse> GetCountry_ReturnsSuccessResult_Inner(Country country)
    {
        var client = _factory.GetClient();

        var response = await client.GetAsync($"/api/v1/country/{country.Id}");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<GetCountryQueryResponse>(responseString);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(country.Name);
        return result;
    }
    
    [Fact]
    public async Task ChangeCountryName_ReturnsSuccessResult()
    {
        var client = await _factory.GetAuthencticatedClient();
        var rnd = new Random();
        var randomString = rnd.Next(1000, 10000);

        var country = SeedData.Countries.First();
        country.Name = $"New Name - {randomString}";

        var changeNameCommand = country.Adapt<ChangeNameCommand>();

        var response = await client.PostAsJsonAsync("/api/v1/country/changename", changeNameCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await GetCountry_ReturnsSuccessResult_Inner(country);
    }
}
