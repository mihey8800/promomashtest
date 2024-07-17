using AutoFixture;
using AutoFixture.AutoMoq;
using Mapster;
using MediatR;
using Moq;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Features.Countries.ChangeName;
using Promomash.Domain.Entities;
using Shouldly;

namespace Promomash.Tests.Features.Countries.ChangeName;

public class ChangeNameCommandTests
{
    private readonly ChangeCountryNameCommandHandler _sut;
    private readonly Mock<ICountryRepository> _countryRepository;

    public ChangeNameCommandTests()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());
        _countryRepository = fixture.Freeze<Mock<ICountryRepository>>();
        _sut = fixture.Create<ChangeCountryNameCommandHandler>();
    }

    [Fact]
    public async Task ChangeCountryName_WithExistingCountry_ReturnsSucceess()
    {
        var changeNameCommand = new ChangeNameCommand
        {
            Id = Guid.NewGuid(),
            Name = "Fashion"
        };
        var country = changeNameCommand.Adapt<Country>();

        _countryRepository.Setup(c => c.GetById(changeNameCommand.Id, null)).ReturnsAsync(country)
            .Verifiable();
        _countryRepository.Setup(c => c.Exists(x => x.Name.ToLower() == It.IsAny<string>(),
                null))
            .ReturnsAsync(false);
        var result = await _sut.Handle(changeNameCommand, CancellationToken.None);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(Unit.Value);
        _countryRepository.Verify(c => c.GetById(changeNameCommand.Id, null), Times.Once);
        _countryRepository.Verify(c => c.Update(country), Times.Once);
    }

    [Fact]
    public async Task ChangeCountryName_WithNonExistingCountry_ReturnsFailure()
    {
        var changeNameCommand = new ChangeNameCommand
        {
            Id = Guid.NewGuid(),
            Name = "Washington"
        };
        var country = changeNameCommand.Adapt<Country>();

        _countryRepository.Setup(c => c.GetById(changeNameCommand.Id, null)).ReturnsAsync(null as Country)
            .Verifiable();

        var result = await _sut.Handle(changeNameCommand, CancellationToken.None);

        result.ShouldNotBeNull();
        result.IsFailed.ShouldBeTrue();
        result.Errors.Any(c => c.Message == $"Country with id {changeNameCommand.Id} cannot be found").ShouldBeTrue();
        _countryRepository.Verify(c => c.GetById(changeNameCommand.Id, null), Times.Once);
        _countryRepository.Verify(c => c.Update(country), Times.Never);
    }
}