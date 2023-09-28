using formulaOne.Entities.DbSets;

namespace formulaOne.DataService.Repositories.Interfaces;

public interface IAchievementRepository : IGenericRepository<Achievement>
{
    Task<IEnumerable<Achievement>> GetDriversAchievementsByIdAsync(Guid id);
}
