using NBT.Application.Authentication.DTOs;

namespace NBT.Application.Authentication.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(Guid userId, string email, IList<string> roles);
    string GenerateRefreshToken();
    Task<LoginResponse> ValidateRefreshTokenAsync(string token, string refreshToken);
}
