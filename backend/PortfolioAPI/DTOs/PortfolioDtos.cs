namespace PortfolioAPI.DTOs;

// ── Project DTOs ───────────────────────────────────────────────────────────

public record ProjectDto(
    int     Id,
    string  Title,
    string  Description,
    string  TechStack,
    string? GithubUrl,
    string? LiveUrl,
    string? ImageUrl,
    bool    IsFeatured,
    DateTime CreatedAt
);

public record CreateProjectDto(
    string  Title,
    string  Description,
    string  TechStack,
    string? GithubUrl,
    string? LiveUrl,
    string? ImageUrl,
    bool    IsFeatured
);

public record UpdateProjectDto(
    string  Title,
    string  Description,
    string  TechStack,
    string? GithubUrl,
    string? LiveUrl,
    string? ImageUrl,
    bool    IsFeatured
);

// ── Skill DTOs ─────────────────────────────────────────────────────────────

public record SkillDto(int Id, string Name, string Category, int Proficiency, string? IconClass);

public record CreateSkillDto(string Name, string Category, int Proficiency, string? IconClass);

// ── Experience DTOs ────────────────────────────────────────────────────────

public record ExperienceDto(
    int      Id,
    string   Company,
    string   Role,
    string   Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool     IsCurrent,
    string?  Location,
    string?  CompanyUrl
);

public record CreateExperienceDto(
    string   Company,
    string   Role,
    string   Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool     IsCurrent,
    string?  Location,
    string?  CompanyUrl
);

// ── Contact DTOs ───────────────────────────────────────────────────────────

public record ContactRequestDto(
    string Name,
    string Email,
    string Subject,
    string Message
);

public record ContactResponseDto(int Id, string Name, string Email, string Subject, string Message, DateTime SentAt, bool IsRead);
