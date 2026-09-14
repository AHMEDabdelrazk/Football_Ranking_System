using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballRanking.Core.Entities;

public enum CompetitionType
{
    League,
    Cup,
    International
}
public class Competition
{
    public int CompetitionId { get; set; }

    public string Name { get; set; } = string.Empty;

    public CompetitionType Type { get; set; }

    public string? Country { get; set; }

    public string Season { get; set; } = string.Empty;

    public string? Code { get; set; }

    public decimal CoefficientWeight { get; set; } = 1.0m;

    public string? Logo { get; set; }

    // Navigation Property
    public ICollection<Club> Clubs { get; set; } = new List<Club>();
}