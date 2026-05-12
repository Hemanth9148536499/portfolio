namespace PortfolioAPI.Models;

public class Experience
{
    public int      Id          { get; set; }
    public string   Company     { get; set; } = string.Empty;
    public string   Role        { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public DateTime StartDate   { get; set; }
    public DateTime? EndDate    { get; set; }          // null = current
    public bool     IsCurrent   { get; set; } = false;
    public string?  Location    { get; set; }
    public string?  CompanyUrl  { get; set; }
}
