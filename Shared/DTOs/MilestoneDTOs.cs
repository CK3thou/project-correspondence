using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class MilestoneDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsAchieved { get; set; }
    public bool IsApproved { get; set; }
    public string? ApprovalComments { get; set; }
    public DateTime? AchievedDate { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedByUserId { get; set; }
    public string? ApprovedByUserName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateMilestoneRequest
{
    [Required]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }
}

public class UpdateMilestoneRequest
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsAchieved { get; set; }
}

public class ApproveMilestoneRequest
{
    [Required]
    public bool IsApproved { get; set; }

    [StringLength(500)]
    public string? ApprovalComments { get; set; }
}
