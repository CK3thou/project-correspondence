using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Milestone
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public bool IsAchieved { get; set; } = false;

    public bool IsApproved { get; set; } = false;

    public string? ApprovalComments { get; set; }

    public DateTime? AchievedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? ApprovedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Project? Project { get; set; }
    public ApplicationUser? ApprovedByUser { get; set; }
}
