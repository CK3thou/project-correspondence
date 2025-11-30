using Blazored.LocalStorage;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UserService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private async Task SetAuthHeader()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        await SetAuthHeader();
        try
        {
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("/api/users");
            return users ?? new List<UserDto>();
        }
        catch
        {
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> GetUser(string id)
    {
        await SetAuthHeader();
        try
        {
            return await _httpClient.GetFromJsonAsync<UserDto>($"/api/users/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<UserResponse> CreateUser(CreateUserRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/users", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result ?? new UserResponse { Success = false, Message = "Unknown error" };
            }
            
            var errorResult = await response.Content.ReadFromJsonAsync<UserResponse>();
            return errorResult ?? new UserResponse { Success = false, Message = "Failed to create user" };
        }
        catch (Exception ex)
        {
            return new UserResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task<UserResponse> UpdateUserRoles(string id, UpdateUserRolesRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/users/{id}/roles", request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UserResponse>();
                return result ?? new UserResponse { Success = false, Message = "Unknown error" };
            }
            
            var errorResult = await response.Content.ReadFromJsonAsync<UserResponse>();
            return errorResult ?? new UserResponse { Success = false, Message = "Failed to update roles" };
        }
        catch (Exception ex)
        {
            return new UserResponse { Success = false, Message = ex.Message };
        }
    }

    public async Task<bool> DeleteUser(string id)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetRoles()
    {
        await SetAuthHeader();
        try
        {
            var roles = await _httpClient.GetFromJsonAsync<IEnumerable<string>>("/api/users/roles");
            return roles ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
