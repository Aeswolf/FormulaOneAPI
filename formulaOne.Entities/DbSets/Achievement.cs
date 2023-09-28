using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace formulaOne.Entities.DbSets;

public class Achievement : BaseEntity
{
    public int RaceWins { get; set; }

    public int WorldChampionshipTitles { get; set; }

    public int PolePosition { get; set; }

    [Required]
    [ForeignKey("Drivers")]
    public Guid DriverId { get; set; }

    public virtual Driver? Driver { get; set; }
}