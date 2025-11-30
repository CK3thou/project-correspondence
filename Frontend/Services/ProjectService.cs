using Blazored.LocalStorage;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services;

public class ProjectService : IProjectService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ProjectService(HttpClient httpClient, ILocalStorageService localStorage)
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

    public async Task<IEnumerable<ProjectDto>> GetProjects()
    {
        await SetAuthHeader();
        try
        {
            var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDto>>("/api/projects");
            return projects ?? new List<ProjectDto>();
        }
        catch
        {
            return new List<ProjectDto>();
        }
    }

    public async Task<ProjectDto?> GetProject(int id)
    {
        await SetAuthHeader();
        try
        {
            return await _httpClient.GetFromJsonAsync<ProjectDto>($"/api/projects/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<ProjectDto?> CreateProject(CreateProjectRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/projects", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProjectDto>();
            }
            
            // Log the error response
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error creating project: {response.StatusCode} - {errorContent}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception creating project: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> UpdateProject(int id, UpdateProjectRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/projects/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteProject(int id)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/projects/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
