using Shared.DTOs;

namespace Frontend.Services;

public interface IAuthService
{
    Task<AuthResponse> Login(LoginRequest request);
    Task<AuthResponse> Register(RegisterRequest request);
    Task Logout();
    Task<bool> IsAuthenticated();
    Task<string?> GetToken();
}
