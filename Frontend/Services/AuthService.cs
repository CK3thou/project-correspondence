using Blazored.LocalStorage;
using Shared.DTOs;
using System.Net.Http.Json;

namespace Frontend.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<AuthResponse> Login(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result?.Success == true && result.Token != null)
        {
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("userEmail", result.Email);
            await _localStorage.SetItemAsync("userName", result.FullName);
            await _localStorage.SetItemAsync("userRole", result.Role);
        }

        return result ?? new AuthResponse { Success = false, Message = "Unknown error" };
    }

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        if (result?.Success == true && result.Token != null)
        {
            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("userEmail", result.Email);
            await _localStorage.SetItemAsync("userName", result.FullName);
            await _localStorage.SetItemAsync("userRole", result.Role);
        }

        return result ?? new AuthResponse { Success = false, Message = "Unknown error" };
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userEmail");
        await _localStorage.RemoveItemAsync("userName");
        await _localStorage.RemoveItemAsync("userRole");
    }

    public async Task<bool> IsAuthenticated()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }
}
