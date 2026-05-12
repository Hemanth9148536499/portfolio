using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.DTOs;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProjectsController : ControllerBase
{
    private readonly PortfolioDbContext _db;
    public ProjectsController(PortfolioDbContext db) => _db = db;

    // GET api/projects
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectDto>), 200)]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(
        [FromQuery] bool? featured = null)
    {
        var query = _db.Projects.AsQueryable();

        if (featured.HasValue)
            query = query.Where(p => p.IsFeatured == featured.Value);

        var projects = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => ToDto(p))
            .ToListAsync();

        return Ok(projects);
    }

    // GET api/projects/5
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProjectDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        return project is null ? NotFound() : Ok(ToDto(project));
    }

    // POST api/projects
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<ProjectDto>> Create([FromBody] CreateProjectDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var project = new Project
        {
            Title       = dto.Title,
            Description = dto.Description,
            TechStack   = dto.TechStack,
            GithubUrl   = dto.GithubUrl,
            LiveUrl     = dto.LiveUrl,
            ImageUrl    = dto.ImageUrl,
            IsFeatured  = dto.IsFeatured,
            CreatedAt   = DateTime.UtcNow
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = project.Id }, ToDto(project));
    }

    // PUT api/projects/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProjectDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ProjectDto>> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        project.Title       = dto.Title;
        project.Description = dto.Description;
        project.TechStack   = dto.TechStack;
        project.GithubUrl   = dto.GithubUrl;
        project.LiveUrl     = dto.LiveUrl;
        project.ImageUrl    = dto.ImageUrl;
        project.IsFeatured  = dto.IsFeatured;

        await _db.SaveChangesAsync();
        return Ok(ToDto(project));
    }

    // DELETE api/projects/5
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _db.Projects.FindAsync(id);
        if (project is null) return NotFound();

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ProjectDto ToDto(Project p) => new(
        p.Id, p.Title, p.Description, p.TechStack,
        p.GithubUrl, p.LiveUrl, p.ImageUrl, p.IsFeatured, p.CreatedAt);
}
