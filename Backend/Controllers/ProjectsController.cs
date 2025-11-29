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
public class ProjectsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var projects = await _context.Projects
            .Include(p => p.CreatedByUser)
            .Include(p => p.Attachments)
            .Include(p => p.Milestones)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var projectDtos = projects.Select(p => MapToDto(p)).ToList();

        return Ok(projectDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(int id)
    {
        var project = await _context.Projects
            .Include(p => p.CreatedByUser)
            .Include(p => p.Attachments)
            .Include(p => p.Milestones)
                .ThenInclude(m => m.ApprovedByUser)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(MapToDto(project));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProjectRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var project = new Project
        {
            Name = request.Name,
            ProjectLink = request.ProjectLink,
            Description = request.Description,
            ApproverName = request.ApproverName,
            NumberOfMilestones = request.NumberOfMilestones,
            CreatedByUserId = userId,
            Status = "Pending"
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Reload with navigation properties
        await _context.Entry(project).Reference(p => p.CreatedByUser).LoadAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, MapToDto(project));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequest request)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (project.CreatedByUserId != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        project.Name = request.Name;
        project.ProjectLink = request.ProjectLink;
        project.Description = request.Description;
        project.ApproverName = request.ApproverName;
        project.Status = request.Status;
        project.NumberOfMilestones = request.NumberOfMilestones;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ProjectManagerOrAdmin")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/attachments")]
    public async Task<ActionResult<ProjectAttachmentDto>> UploadAttachment(int id, IFormFile file)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            return NotFound();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        // Create uploads directory if it doesn't exist
        var uploadsPath = Path.Combine("uploads", "projects", id.ToString());
        Directory.CreateDirectory(uploadsPath);

        // Generate unique filename
        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new ProjectAttachment
        {
            ProjectId = id,
            FileName = file.FileName,
            FilePath = filePath,
            ContentType = file.ContentType,
            FileSize = file.Length
        };

        _context.ProjectAttachments.Add(attachment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, new ProjectAttachmentDto
        {
            Id = attachment.Id,
            ProjectId = attachment.ProjectId,
            FileName = attachment.FileName,
            FilePath = attachment.FilePath,
            ContentType = attachment.ContentType,
            FileSize = attachment.FileSize,
            UploadedAt = attachment.UploadedAt
        });
    }

    private static ProjectDto MapToDto(Project project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            ProjectLink = project.ProjectLink,
            Description = project.Description,
            ApproverName = project.ApproverName,
            Status = project.Status,
            NumberOfMilestones = project.NumberOfMilestones,
            CreatedByUserId = project.CreatedByUserId,
            CreatedByUserName = project.CreatedByUser?.FullName,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Attachments = project.Attachments?.Select(a => new ProjectAttachmentDto
            {
                Id = a.Id,
                ProjectId = a.ProjectId,
                FileName = a.FileName,
                FilePath = a.FilePath,
                ContentType = a.ContentType,
                FileSize = a.FileSize,
                UploadedAt = a.UploadedAt
            }).ToList() ?? new List<ProjectAttachmentDto>(),
            Milestones = project.Milestones?.Select(m => new MilestoneDto
            {
                Id = m.Id,
                ProjectId = m.ProjectId,
                Name = m.Name,
                Description = m.Description,
                IsAchieved = m.IsAchieved,
                IsApproved = m.IsApproved,
                ApprovalComments = m.ApprovalComments,
                AchievedDate = m.AchievedDate,
                ApprovedDate = m.ApprovedDate,
                ApprovedByUserId = m.ApprovedByUserId,
                ApprovedByUserName = m.ApprovedByUser?.FullName,
                CreatedAt = m.CreatedAt
            }).ToList() ?? new List<MilestoneDto>()
        };
    }
}
