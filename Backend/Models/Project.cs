using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Project
{
    [Key]
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
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Completed, Rejected

    public int NumberOfMilestones { get; set; }

    [Required]
    public string CreatedByUserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public ApplicationUser? CreatedByUser { get; set; }
    public ICollection<ProjectAttachment> Attachments { get; set; } = new List<ProjectAttachment>();
    public ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();
}
