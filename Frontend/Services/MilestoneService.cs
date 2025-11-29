using Blazored.LocalStorage;
using Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Frontend.Services;

public class MilestoneService : IMilestoneService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public MilestoneService(HttpClient httpClient, ILocalStorageService localStorage)
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

    public async Task<IEnumerable<MilestoneDto>> GetMilestonesByProject(int projectId)
    {
        await SetAuthHeader();
        try
        {
            var milestones = await _httpClient.GetFromJsonAsync<IEnumerable<MilestoneDto>>($"/api/milestones/project/{projectId}");
            return milestones ?? new List<MilestoneDto>();
        }
        catch
        {
            return new List<MilestoneDto>();
        }
    }

    public async Task<MilestoneDto?> GetMilestone(int id)
    {
        await SetAuthHeader();
        try
        {
            return await _httpClient.GetFromJsonAsync<MilestoneDto>($"/api/milestones/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<MilestoneDto?> CreateMilestone(CreateMilestoneRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/milestones", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<MilestoneDto>();
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateMilestone(int id, UpdateMilestoneRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/milestones/{id}", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ApproveMilestone(int id, ApproveMilestoneRequest request)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/milestones/{id}/approve", request);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteMilestone(int id)
    {
        await SetAuthHeader();
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/milestones/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
