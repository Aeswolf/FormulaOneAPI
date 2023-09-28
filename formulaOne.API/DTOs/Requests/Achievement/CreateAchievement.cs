using System.ComponentModel.DataAnnotations;


namespace formulaOne.API.DTOs.Requests.Achievement;

public record CreateAchievementDto
{
    public int RaceWins { get; set; }

    public int WorldChampionshipTitles { get; set; }

    public int PolePosition { get; set; }

    [Required]
    public Guid DriverId { get; set; }
}