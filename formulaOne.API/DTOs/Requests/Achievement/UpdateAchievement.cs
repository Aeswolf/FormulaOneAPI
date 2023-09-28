using System.ComponentModel.DataAnnotations;

namespace formulaOne.API.DTOs.Requests;

public record UpdateAchievementDto
{
    public Guid Id { get; set; }

    public int RaceWins { get; set; }

    public int WorldChampionshipTitles { get; set; }

    public int PolePosition { get; set; }

    [Required]
    public Guid DriverId { get; set; }
}