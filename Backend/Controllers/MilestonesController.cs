using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MilestonesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MilestonesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<MilestoneDto>>> GetMilestonesByProject(int projectId)
    {
        var milestones = await _context.Milestones
            .Include(m => m.ApprovedByUser)
            .Where(m => m.ProjectId == projectId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var milestoneDtos = milestones.Select(m => MapToDto(m)).ToList();

        return Ok(milestoneDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MilestoneDto>> GetMilestone(int id)
    {
        var milestone = await _context.Milestones
            .Include(m => m.ApprovedByUser)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (milestone == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(milestone));
    }

    [HttpPost]
    public async Task<ActionResult<MilestoneDto>> CreateMilestone([FromBody] CreateMilestoneRequest request)
    {
        var project = await _context.Projects.FindAsync(request.ProjectId);

        if (project == null)
        {
            return NotFound("Project not found");
        }

        var milestone = new Milestone
        {
            ProjectId = request.ProjectId,
            Name = request.Name,
            Description = request.Description
        };

        _context.Milestones.Add(milestone);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMilestone), new { id = milestone.Id }, MapToDto(milestone));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMilestone(int id, [FromBody] UpdateMilestoneRequest request)
    {
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return NotFound();
        }

        milestone.Name = request.Name;
        milestone.Description = request.Description;
        milestone.IsAchieved = request.IsAchieved;

        if (request.IsAchieved && milestone.AchievedDate == null)
        {
            milestone.AchievedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = "ProjectManagerOrAdmin")]
    public async Task<IActionResult> ApproveMilestone(int id, [FromBody] ApproveMilestoneRequest request)
    {
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return NotFound();
        }

        if (!milestone.IsAchieved)
        {
            return BadRequest("Milestone must be achieved before it can be approved");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        milestone.IsApproved = request.IsApproved;
        milestone.ApprovalComments = request.ApprovalComments;
        milestone.ApprovedDate = DateTime.UtcNow;
        milestone.ApprovedByUserId = userId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectManagerOrAdmin")]
    public async Task<IActionResult> DeleteMilestone(int id)
    {
        var milestone = await _context.Milestones.FindAsync(id);

        if (milestone == null)
        {
            return NotFound();
        }

        _context.Milestones.Remove(milestone);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static MilestoneDto MapToDto(Milestone milestone)
    {
        return new MilestoneDto
        {
            Id = milestone.Id,
            ProjectId = milestone.ProjectId,
            Name = milestone.Name,
            Description = milestone.Description,
            IsAchieved = milestone.IsAchieved,
            IsApproved = milestone.IsApproved,
            ApprovalComments = milestone.ApprovalComments,
            AchievedDate = milestone.AchievedDate,
            ApprovedDate = milestone.ApprovedDate,
            ApprovedByUserId = milestone.ApprovedByUserId,
            ApprovedByUserName = milestone.ApprovedByUser?.FullName,
            CreatedAt = milestone.CreatedAt
        };
    }
}
