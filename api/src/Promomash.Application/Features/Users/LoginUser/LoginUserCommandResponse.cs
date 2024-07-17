using System.Text.Json.Serialization;

namespace Promomash.Application.Features.Users.LoginUser;

public class LoginUserCommandResponse
{
    [JsonPropertyName("access_token")]
    public required string Token { get; set; }
}