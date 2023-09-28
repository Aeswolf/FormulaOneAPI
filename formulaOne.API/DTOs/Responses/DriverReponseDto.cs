namespace formulaOne.API.DTOs.Responses;

public record DriverResponseDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string DateOfBirth { get; set; } = string.Empty;

    public int DriverNumber { get; set; }

    public ICollection<AchievementResponseDto> Achievements { get; set; } = new List<AchievementResponseDto>();
}