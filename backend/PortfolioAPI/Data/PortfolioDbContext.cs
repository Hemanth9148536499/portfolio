using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Models;

namespace PortfolioAPI.Data;

public class PortfolioDbContext : DbContext
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

    public DbSet<Project>        Projects        { get; set; }
    public DbSet<Skill>          Skills          { get; set; }
    public DbSet<Experience>     Experiences     { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Seed data ──────────────────────────────────────────────────────

        modelBuilder.Entity<Project>().HasData(
            new Project
            {
                Id          = 1,
                Title       = "E-Commerce Platform",
                Description = "Full-stack e-commerce solution with cart, payments (Stripe), and admin dashboard.",
                TechStack   = "ASP.NET Core,React,MySQL,Stripe",
                GithubUrl   = "https://github.com/yourusername/ecommerce",
                LiveUrl     = "https://mystore.example.com",
                IsFeatured  = true,
                CreatedAt   = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Project
            {
                Id          = 2,
                Title       = "Task Manager API",
                Description = "RESTful task management API with JWT auth, role-based access, and real-time updates.",
                TechStack   = "C#,.NET 8,MySQL,SignalR,JWT",
                GithubUrl   = "https://github.com/yourusername/taskapi",
                IsFeatured  = true,
                CreatedAt   = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Project
            {
                Id          = 3,
                Title       = "Portfolio CMS",
                Description = "This portfolio — a headless CMS backed by ASP.NET Core APIs and a static GitHub Pages frontend.",
                TechStack   = "ASP.NET Core,MySQL,HTML,CSS,JavaScript",
                GithubUrl   = "https://github.com/yourusername/portfolio",
                IsFeatured  = false,
                CreatedAt   = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Skill>().HasData(
            new Skill { Id = 1, Name = "C#",             Category = "Backend",  Proficiency = 92 },
            new Skill { Id = 2, Name = "ASP.NET Core",   Category = "Backend",  Proficiency = 90 },
            new Skill { Id = 3, Name = "MySQL",          Category = "Database", Proficiency = 85 },
            new Skill { Id = 4, Name = "Entity Framework", Category = "Backend", Proficiency = 88 },
            new Skill { Id = 5, Name = "JavaScript",     Category = "Frontend", Proficiency = 80 },
            new Skill { Id = 6, Name = "React",          Category = "Frontend", Proficiency = 75 },
            new Skill { Id = 7, Name = "Docker",         Category = "DevOps",   Proficiency = 70 },
            new Skill { Id = 8, Name = "Git",            Category = "DevOps",   Proficiency = 90 },
            new Skill { Id = 9, Name = "REST APIs",      Category = "Backend",  Proficiency = 95 },
            new Skill { Id = 10, Name = "HTML/CSS",      Category = "Frontend", Proficiency = 85 }
        );

        modelBuilder.Entity<Experience>().HasData(
            new Experience
            {
                Id          = 1,
                Company     = "Tech Corp Inc.",
                Role        = "Senior .NET Developer",
                Description = "Led development of microservices architecture. Mentored junior developers. Reduced API response time by 40% through query optimisation.",
                StartDate   = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsCurrent   = true,
                Location    = "Remote"
            },
            new Experience
            {
                Id          = 2,
                Company     = "StartupXYZ",
                Role        = "Full-Stack Developer",
                Description = "Built and maintained customer-facing web applications using ASP.NET Core and React.",
                StartDate   = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate     = new DateTime(2021, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                IsCurrent   = false,
                Location    = "Bangalore, India"
            }
        );
    }
}
