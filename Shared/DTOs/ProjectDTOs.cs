using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class ProjectDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ProjectLink { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string ApproverName { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Pending";

    public int NumberOfMilestones { get; set; }

    public string? CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<ProjectAttachmentDto> Attachments { get; set; } = new();
    public List<MilestoneDto> Milestones { get; set; } = new();
}

public class CreateProjectRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ProjectLink { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string ApproverName { get; set; } = string.Empty;

    [Required]
    [Range(1, 100)]
    public int NumberOfMilestones { get; set; }
}

public class UpdateProjectRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ProjectLink { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string ApproverName { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = "Pending";

    [Range(1, 100)]
    public int NumberOfMilestones { get; set; }
}

public class ProjectAttachmentDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string? ContentType { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadedAt { get; set; }
}
