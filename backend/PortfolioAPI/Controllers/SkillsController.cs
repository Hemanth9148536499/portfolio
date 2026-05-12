using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.DTOs;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SkillsController : ControllerBase
{
    private readonly PortfolioDbContext _db;
    public SkillsController(PortfolioDbContext db) => _db = db;

    // GET api/skills
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SkillDto>>> GetAll(
        [FromQuery] string? category = null)
    {
        var query = _db.Skills.AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(s => s.Category == category);

        var skills = await query
            .OrderByDescending(s => s.Proficiency)
            .Select(s => new SkillDto(s.Id, s.Name, s.Category, s.Proficiency, s.IconClass))
            .ToListAsync();

        return Ok(skills);
    }

    // GET api/skills/categories
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategories()
    {
        var categories = await _db.Skills
            .Select(s => s.Category)
            .Distinct()
            .ToListAsync();
        return Ok(categories);
    }

    // GET api/skills/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SkillDto>> GetById(int id)
    {
        var skill = await _db.Skills.FindAsync(id);
        return skill is null ? NotFound()
            : Ok(new SkillDto(skill.Id, skill.Name, skill.Category, skill.Proficiency, skill.IconClass));
    }

    // POST api/skills
    [HttpPost]
    public async Task<ActionResult<SkillDto>> Create([FromBody] CreateSkillDto dto)
    {
        var skill = new Skill
        {
            Name        = dto.Name,
            Category    = dto.Category,
            Proficiency = dto.Proficiency,
            IconClass   = dto.IconClass
        };
        _db.Skills.Add(skill);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = skill.Id },
            new SkillDto(skill.Id, skill.Name, skill.Category, skill.Proficiency, skill.IconClass));
    }

    // DELETE api/skills/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var skill = await _db.Skills.FindAsync(id);
        if (skill is null) return NotFound();
        _db.Skills.Remove(skill);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
