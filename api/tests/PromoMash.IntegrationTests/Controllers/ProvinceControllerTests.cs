using System.Net;
using System.Net.Http.Json;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Promomash.Application.Features.Provinces.ChangeName;
using Promomash.Application.Features.Provinces.CreateProvince;
using Promomash.Application.Features.Provinces.GetProvince;
using Promomash.Application.Features.Provinces.GetProvinces;
using Promomash.Domain.Entities;
using Promomash.EntityFramework.Initialization.Seed;
using Promomash.Host;
using PromoMash.IntegrationTests.Base;
using Shouldly;

namespace PromoMash.IntegrationTests.Controllers;

public class ProvinceControllerTests : IClassFixture<PromomashWebApplicationFactory<Program>>
{
    private readonly PromomashWebApplicationFactory<Program> _factory;

    public ProvinceControllerTests(PromomashWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetProvinces_ReturnsSuccessResult()
    {
        var client = _factory.GetClient();

        var response = await client.GetAsync("/api/v1/province");
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<GetProvincesQueryResponse>>(responseString);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetProvince_ReturnsSuccessResult()
    {
        var province = SeedData.Countries.First().Provinces.First();
        await GetProvince_ReturnsSuccessResult_Inner(province);
    }

    [Fact]
    public async Task<Province> CreateProvince_ReturnsSuccessResult()
    {
        var client = await _factory.GetAuthencticatedClient();
        var rnd = new Random();
        var randomString = rnd.Next(1000, 10000);

        var createProvinceCommand = new CreateProvinceCommand
        {
            Name = $"Test Province-{randomString}",
            CountryId = SeedData.Countries.First().Id
        };

        var response = await client.PostAsJsonAsync("/api/v1/province", createProvinceCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<CreateProvinceCommandResponse>(responseString);
        result.ShouldNotBeNull();

        var createProvinceReturnsSuccessResult = createProvinceCommand.Adapt<Province>();
        createProvinceReturnsSuccessResult.Id = result.Id;

        await GetProvince_ReturnsSuccessResult_Inner(createProvinceReturnsSuccessResult);

        return createProvinceReturnsSuccessResult;
    }

    [Fact]
    public async Task CreateProvince_ReturnsBadRequestResult()
    {
        var client = await _factory.GetAuthencticatedClient();

        var createProvinceCommand = new CreateProvinceCommand
        {
            Name = null
        };
        var response = await client.PostAsJsonAsync("/api/v1/province", createProvinceCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ProblemDetails>(responseString);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task ChangeProvinceName_ReturnsSuccessResult()
    {
        var client = await _factory.GetAuthencticatedClient();
        var rnd = new Random();
        var randomString = rnd.Next(1000, 10000);

        var province = SeedData.Countries.First().Provinces.First();
        province.Name = $"New Name - {randomString}";

        var changeNameCommand = province.Adapt<ChangeNameCommand>();

        var response = await client.PostAsJsonAsync("/api/v1/province/changename", changeNameCommand);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        await GetProvince_ReturnsSuccessResult_Inner(province);
    }

    [Fact]
    public async Task DeleteProvince_ReturnsBadRequestResult()
    {
        var provinceReturnsSuccessResult = await CreateProvince_ReturnsSuccessResult();
        var client = await _factory.GetAuthencticatedClient();

        var response = await client.DeleteAsync($"/api/v1/province/{provinceReturnsSuccessResult.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var getResponse = await client.GetAsync($"/api/v1/province/{provinceReturnsSuccessResult.Id}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    private async Task<GetProvinceQueryResponse> GetProvince_ReturnsSuccessResult_Inner(Province province)
    {
        var client = _factory.GetClient();

        var response = await client.GetAsync($"/api/v1/province/{province.Id}");

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<GetProvinceQueryResponse>(responseString);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(province.Name);
        return result;
    }
}