using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.DTOs;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ExperienceController : ControllerBase
{
    private readonly PortfolioDbContext _db;
    public ExperienceController(PortfolioDbContext db) => _db = db;

    // GET api/experience
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetAll()
    {
        var list = await _db.Experiences
            .OrderByDescending(e => e.StartDate)
            .Select(e => ToDto(e))
            .ToListAsync();
        return Ok(list);
    }

    // GET api/experience/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExperienceDto>> GetById(int id)
    {
        var exp = await _db.Experiences.FindAsync(id);
        return exp is null ? NotFound() : Ok(ToDto(exp));
    }

    // POST api/experience
    [HttpPost]
    public async Task<ActionResult<ExperienceDto>> Create([FromBody] CreateExperienceDto dto)
    {
        var exp = new Experience
        {
            Company     = dto.Company,
            Role        = dto.Role,
            Description = dto.Description,
            StartDate   = dto.StartDate,
            EndDate     = dto.EndDate,
            IsCurrent   = dto.IsCurrent,
            Location    = dto.Location,
            CompanyUrl  = dto.CompanyUrl
        };
        _db.Experiences.Add(exp);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = exp.Id }, ToDto(exp));
    }

    // PUT api/experience/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ExperienceDto>> Update(int id, [FromBody] CreateExperienceDto dto)
    {
        var exp = await _db.Experiences.FindAsync(id);
        if (exp is null) return NotFound();

        exp.Company     = dto.Company;
        exp.Role        = dto.Role;
        exp.Description = dto.Description;
        exp.StartDate   = dto.StartDate;
        exp.EndDate     = dto.EndDate;
        exp.IsCurrent   = dto.IsCurrent;
        exp.Location    = dto.Location;
        exp.CompanyUrl  = dto.CompanyUrl;

        await _db.SaveChangesAsync();
        return Ok(ToDto(exp));
    }

    // DELETE api/experience/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exp = await _db.Experiences.FindAsync(id);
        if (exp is null) return NotFound();
        _db.Experiences.Remove(exp);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static ExperienceDto ToDto(Experience e) => new(
        e.Id, e.Company, e.Role, e.Description,
        e.StartDate, e.EndDate, e.IsCurrent, e.Location, e.CompanyUrl);
}
