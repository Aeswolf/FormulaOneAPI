using System.Data.Common;
using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace formulaOne.DataService.Repositories.Implementations;

public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
{

    public AchievementRepository(APIDbContext context, ILogger logger) : base(context, logger) { }

    public override async Task<Achievement> AddAsync(Achievement achievement)
    {
        achievement.Id = Guid.NewGuid();
        achievement.CreatedAt = DateTime.UtcNow;
        achievement.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _dbSet!.AddAsync(achievement);
            return achievement;
        }
        catch (DbException)
        {
            _logger.Error("Error occurred while adding a new achievement to the database");
            throw;
        }
    }

    public override async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            Achievement? achievement = await _dbSet!.FirstOrDefaultAsync(a => a.IsDeleted == false && a.Id == id);

            if (achievement is null) return false;

            achievement.IsDeleted = true;
            achievement.DeletedAt = DateTime.UtcNow;

            return true;
        }
        catch (DbException)
        {
            _logger.Error("Error occurred while deleting an achievement from the database");
            throw;
        }
    }

    public override async Task<Achievement?> GetByIdAsync(Guid id)
    {
        try
        {
            Achievement? achievement = await _dbSet!.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);

            return achievement;
        }
        catch (DbException)
        {
            _logger.Error("Error occurred while retrieving an achievement from the database");
            throw;
        }
    }

    public override async Task<IEnumerable<Achievement>> GetAllAsync()
    {
        try
        {
            return await _dbSet!.Where(a => a.IsDeleted == false)
                        .AsNoTracking()
                        .AsSplitQuery()
                        .OrderBy(a => a.CreatedAt)
                        .ToListAsync();
        }
        catch (DbException)
        {
            _logger.Error("Error retrieving achievements from the database");
            throw;
        }
    }

    public override async Task<Achievement?> UpdateAsync(Guid id, Achievement achievement)
    {
        try
        {
            Achievement? existingAchievement = await _dbSet!.FirstOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);

            if (existingAchievement is null) return null;

            existingAchievement.DriverId = achievement.DriverId;
            existingAchievement.PolePosition = achievement.PolePosition;
            existingAchievement.RaceWins = achievement.RaceWins;
            existingAchievement.UpdatedAt = DateTime.UtcNow;

            return existingAchievement;
        }
        catch (DbUpdateException)
        {
            _logger.Error("Error occurred while updating an achievement instance in the database");
            throw;
        }
    }


    public async Task<IEnumerable<Achievement>> GetDriversAchievementsByIdAsync(Guid id)
    {
        try
        {
            return await _dbSet!.Where(a => a.DriverId == id && a.IsDeleted == false)
                        .AsNoTracking()
                        .AsSplitQuery()
                        .OrderBy(a => a.CreatedAt)
                        .ToListAsync();
        }
        catch (DbException)
        {
            _logger.Error("Error occurred while retrieving achievement based on a driver's id from the database");
            throw;
        }
    }
}