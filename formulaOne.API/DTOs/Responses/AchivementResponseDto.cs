namespace formulaOne.API.DTOs.Responses;

public record AchievementResponseDto
{
    public Guid Id { get; set; }

    public int RaceWins { get; set; }

    public int WorldChampionshipTitles { get; set; }

    public int PolePosition { get; set; }

}