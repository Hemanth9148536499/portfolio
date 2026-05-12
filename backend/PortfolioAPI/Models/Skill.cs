namespace PortfolioAPI.Models;

public class Skill
{
    public int    Id         { get; set; }
    public string Name       { get; set; } = string.Empty;
    public string Category   { get; set; } = string.Empty;   // e.g. "Backend", "Frontend", "DevOps"
    public int    Proficiency { get; set; } = 50;             // 0–100
    public string? IconClass { get; set; }                    // e.g. devicon class
}
