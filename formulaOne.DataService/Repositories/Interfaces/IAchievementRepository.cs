using formulaOne.Entities.DbSets;

namespace formulaOne.DataService.Repositories.Interfaces;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAllAchievementsAsync();

    Task<Achievement?> GetAchievementByIdAsync(Guid id);

    Task<Achievement> AddAchievementAsync(Achievement achievement);

    Task<Achievement?> UpdateAchievementAsync(Guid id, Achievement achievement);

    Task<bool> DeleteAchievementByIdAsync(Guid id);
}