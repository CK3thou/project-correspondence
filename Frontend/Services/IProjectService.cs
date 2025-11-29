using Shared.DTOs;

namespace Frontend.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetProjects();
    Task<ProjectDto?> GetProject(int id);
    Task<ProjectDto?> CreateProject(CreateProjectRequest request);
    Task<bool> UpdateProject(int id, UpdateProjectRequest request);
    Task<bool> DeleteProject(int id);
}
