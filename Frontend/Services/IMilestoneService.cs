using Shared.DTOs;

namespace Frontend.Services;

public interface IMilestoneService
{
    Task<IEnumerable<MilestoneDto>> GetMilestonesByProject(int projectId);
    Task<MilestoneDto?> GetMilestone(int id);
    Task<MilestoneDto?> CreateMilestone(CreateMilestoneRequest request);
    Task<bool> UpdateMilestone(int id, UpdateMilestoneRequest request);
    Task<bool> ApproveMilestone(int id, ApproveMilestoneRequest request);
    Task<bool> DeleteMilestone(int id);
}
