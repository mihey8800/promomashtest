using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Promomash.Application.Contracts.Authorization;
using Promomash.Domain.Common;
using Promomash.EntityFramework.Common;
using IdentityUser = Promomash.Infrastructure.Authorization.IdentityUser;

namespace Promomash.EntityFramework.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly ICurrentUserService? _currentUserService;
    private readonly IMediator? _mediator;

    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService? currentUserService,
        IMediator? mediator)
        : base(options)
    {
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public DbSet<Province> Provinces => Set<Province>();

    public DbSet<Country> Countries => Set<Country>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(
                "Data Source=.;Initial Catalog=CleanArchitecture;Integrated Security=True;TrustServerCertificate=True;",
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Entity<IdentityUser>().HasOne(x => x.Country);
        modelBuilder.Entity<IdentityUser>().HasOne(x => x.Province);
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(iul => new { iul.LoginProvider, iul.ProviderKey });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(iur => new { iur.UserId, iur.RoleId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(iut => new { iut.UserId, iut.LoginProvider, iut.Name });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await _mediator!.DispatchDomainEvents(this);

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTimeOffset.Now;
                    entry.Entity.CreatedBy = _currentUserService?.Login ?? string.Empty;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTimeOffset.Now;
                    entry.Entity.LastModifiedBy = _currentUserService?.Login ?? string.Empty;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}