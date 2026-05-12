using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.DTOs;
using PortfolioAPI.Models;
using System.Text.RegularExpressions;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly PortfolioDbContext _db;
    public ContactController(PortfolioDbContext db) => _db = db;

    // POST api/contact  — public endpoint for visitors
    [HttpPost]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Send([FromBody] ContactRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)    ||
            string.IsNullOrWhiteSpace(dto.Email)   ||
            string.IsNullOrWhiteSpace(dto.Subject) ||
            string.IsNullOrWhiteSpace(dto.Message))
            return BadRequest(new { error = "All fields are required." });

        if (!IsValidEmail(dto.Email))
            return BadRequest(new { error = "Invalid email address." });

        var message = new ContactMessage
        {
            Name    = dto.Name.Trim(),
            Email   = dto.Email.Trim(),
            Subject = dto.Subject.Trim(),
            Message = dto.Message.Trim(),
            SentAt  = DateTime.UtcNow
        };

        _db.ContactMessages.Add(message);
        await _db.SaveChangesAsync();

        return StatusCode(201, new { message = "Message received! I'll get back to you soon.", id = message.Id });
    }

    // GET api/contact  — admin: list all messages
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactResponseDto>>> GetAll()
    {
        var messages = await _db.ContactMessages
            .OrderByDescending(m => m.SentAt)
            .Select(m => new ContactResponseDto(
                m.Id, m.Name, m.Email, m.Subject, m.Message, m.SentAt, m.IsRead))
            .ToListAsync();
        return Ok(messages);
    }

    // PATCH api/contact/5/read
    [HttpPatch("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id)
    {
        var msg = await _db.ContactMessages.FindAsync(id);
        if (msg is null) return NotFound();
        msg.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static bool IsValidEmail(string email) =>
        Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
}
