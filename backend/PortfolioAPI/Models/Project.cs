namespace PortfolioAPI.Models;

/// <summary>Portfolio project entry.</summary>
public class Project
{
    public int    Id          { get; set; }
    public string Title       { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TechStack   { get; set; } = string.Empty;   // CSV e.g. "C#,React,MySQL"
    public string? GithubUrl  { get; set; }
    public string? LiveUrl    { get; set; }
    public string? ImageUrl   { get; set; }
    public bool   IsFeatured  { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
