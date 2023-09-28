using System.ComponentModel.DataAnnotations;
namespace formulaOne.Entities.DbSets;

public class Driver : BaseEntity
{
    public Driver()
    {
        Achievements = new HashSet<Achievement>();
    }

    [Required(ErrorMessage = "FirstName field can not be empty or null")]
    [MaxLength(50, ErrorMessage = "Characters can not exceed 50")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName field can not be empty or null")]
    [MaxLength(50, ErrorMessage = "Characters can not exceed 50")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of Birth can not be null")]
    public DateOnly DateOfBirth { get; set; }

    public int DriverNumber { get; set; }

    public virtual ICollection<Achievement> Achievements { get; set; }

}