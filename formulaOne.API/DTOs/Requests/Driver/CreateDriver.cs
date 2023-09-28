using System.ComponentModel.DataAnnotations;

namespace formulaOne.API.DTOs.Requests.Driver;

public record CreateDriverDto
{
    [Required(ErrorMessage = "FirstName field can not be empty or null")]
    [MaxLength(50, ErrorMessage = "Characters can not exceed 50")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName field can not be empty or null")]
    [MaxLength(50, ErrorMessage = "Characters can not exceed 50")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Birth can not be null")]
    public string DateOfBirth { get; set; } = string.Empty;

    public int DriverNumber { get; set; }
}