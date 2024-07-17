using System.Transactions;
using MediatR;
using Promomash.Application.Features.Users.CreateUser;
using Promomash.EntityFramework.Context;

namespace Promomash.EntityFramework.Initialization.Seed;

public class SeedProcess : ICustomSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    private static readonly object Locker = new object();

    public bool IsDevelopmentData => false;
    public int Order => 1;

    public SeedProcess(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public Task Initialize()
    {
        lock (Locker)
        {
            foreach (var country in SeedData.Countries)
            {
                _context.SeedData(country).GetAwaiter().GetResult();
            }

            _context.SaveChangesAsync().GetAwaiter().GetResult();

            var province = _context.Provinces.First(x => x.Country.Id == SeedData.Admin.Country.Id);
            _mediator.Send(new CreateUserCommand()
            {
                Login = SeedData.Admin.Login,
                ProvinceId = province.Id,
                Password = SeedData.Admin.PasswordHash,
                CountryId = SeedData.Admin.Country.Id
            }).GetAwaiter().GetResult();
        }

        return Task.CompletedTask;
    }
}