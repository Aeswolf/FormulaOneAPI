using System.ComponentModel.DataAnnotations;
namespace formulaOne.Entities.DbSets;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime DeletedAt { get; set; }

    public bool IsDeleted { get; set; }
}