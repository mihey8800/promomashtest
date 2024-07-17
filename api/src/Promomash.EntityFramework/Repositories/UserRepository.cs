using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Promomash.Application.Contracts.Persistence;
using Promomash.Application.Models;
using Promomash.EntityFramework.Common;
using Promomash.EntityFramework.Context;
using IdentityUser = Promomash.Infrastructure.Authorization.IdentityUser;

namespace Promomash.EntityFramework.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserRepository(ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration) : base(dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public override async Task<bool> Exists(Expression<Func<User, bool>> expression,
        Expression<Func<User, object>>? includePropertiesExpression = null)
    {
        var query = _userManager.Users.AsQueryable();
        if (includePropertiesExpression != null)
        {
            query = ExpressionMappingExtensions<User, IdentityUser>.MapIncludeProperties(query,
                includePropertiesExpression);
        }

        var identityExpression = expression.Adapt<Expression<Func<IdentityUser, bool>>>();
        return await query.AnyAsync(identityExpression);
    }

    public override async Task<User?> GetById(Guid id,
        Expression<Func<User, object>>? includePropertiesExpression = null)
    {
        var query = _userManager.Users.AsQueryable();
        if (includePropertiesExpression != null)
        {
            query = ExpressionMappingExtensions<User, IdentityUser>.MapIncludeProperties(query,
                includePropertiesExpression);
        }

        return (await query
            .FirstOrDefaultAsync(x => x.Id == id.ToString()))?.ToDomainUser();
    }

    public override async Task<PaginatedList<User>> GetPagedReponse(int page,
        int size,
        Expression<Func<User, object>>? includePropertiesExpression = null)
    {
        var query = _userManager.Users.AsQueryable();
        if (includePropertiesExpression != null)
        {
            query = ExpressionMappingExtensions<User, IdentityUser>.MapIncludeProperties(query,
                includePropertiesExpression);
        }

        var items = await query.Skip((page - 1) * size).Take(size).Select(s => s.ToDomainUser()).AsNoTracking()
            .ToListAsync();
        var count = await query.CountAsync();
        return new PaginatedList<User>(items, count, page, size);
    }

    public async Task<Result<User>> Add(User entity)
    {
        var identityUser = IdentityUser.FromDomainUser(entity);
        var creationResult = await _userManager.CreateAsync(identityUser, entity.PasswordHash);
        return creationResult.Succeeded ? entity : Result.Fail(creationResult.Errors.Select(s => s.Description));
    }

    public async Task<IReadOnlyList<User>> ListAll(Expression<Func<User, object>>? includePropertiesExpression = null)
    {
        var query = _userManager.Users.AsQueryable();
        if (includePropertiesExpression != null)
        {
            query = ExpressionMappingExtensions<User, IdentityUser>.MapIncludeProperties(query,
                includePropertiesExpression);
        }

        return await query.Select(s => s.ToDomainUser()).ToListAsync();
    }

    public async Task<IReadOnlyList<User>> ListAllWithoutTracking(
        Expression<Func<User, object>>? includePropertiesExpression = null)
    {
        var query = _userManager.Users.AsQueryable();
        if (includePropertiesExpression != null)
        {
            query = ExpressionMappingExtensions<User, IdentityUser>.MapIncludeProperties(query,
                includePropertiesExpression);
        }

        return await query.Select(s => s.ToDomainUser()).AsNoTracking().ToListAsync();
    }

    public async Task Delete(User entity)
    {
        await _userManager.DeleteAsync(IdentityUser.FromDomainUser(entity));
    }

    public async Task Update(User entity)
    {
        await _userManager.UpdateAsync(IdentityUser.FromDomainUser(entity));
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<User?> Login(string login, string password)
    {
        var loginResult = await _signInManager.PasswordSignInAsync(login, password, false, false);
        var res = (await _signInManager.UserManager.Users.Include(x => x.Country).Include(x => x.Province)
            .FirstOrDefaultAsync(x => x.UserName == login));
        var s = res.ToDomainUser();
        return loginResult.Succeeded
            ? (await _signInManager.UserManager.Users.Include(x => x.Country).Include(x => x.Province)
                .FirstOrDefaultAsync())
            ?.ToDomainUser()
            : null;
    }

    /// <summary>
    /// Logout
    /// </summary>
    /// <returns></returns>
    public async Task<bool> Logout()
    {
        await _signInManager.SignOutAsync();
        return true;
    }

    /// <summary>
    /// Generate GWT token for user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Country, user.Country.Name),
            new Claim(ClaimTypes.StateOrProvince, user.Province.Name),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"] ??
                                                          throw new InvalidOperationException())),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}