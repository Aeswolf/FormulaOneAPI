using System.Data.Common;
using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.EntityFrameworkCore;

namespace formulaOne.DataService.Repositories.Implementations;

public class AchievementRepository : IAchievementRepository
{
    private readonly APIDbContext _context;

    public AchievementRepository(APIDbContext context)
    {
        _context = context;
    }

    public async Task<Achievement> AddAchievementAsync(Achievement achievement)
    {
        achievement.Id = Guid.NewGuid();
        achievement.CreatedAt = DateTime.UtcNow;
        achievement.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.Achievements.AddAsync(achievement);
            await _context.SaveChangesAsync();
            return achievement;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAchievementByIdAsync(Guid id)
    {
        try
        {
            Achievement? achievement = await _context.Achievements.FindAsync(id);

            if (achievement is null) return false;

            achievement.IsDeleted = true;
            achievement.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<Achievement?> GetAchievementByIdAsync(Guid id)
    {
        try
        {
            Achievement? achievement = await _context.Achievements.FirstOrDefaultAsync(a => a.Id == id);

            return achievement;
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Achievement>> GetAllAchievementsAsync()
    {
        try
        {
            return await _context.Achievements.ToListAsync();
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<Achievement?> UpdateAchievementAsync(Guid id, Achievement achievement)
    {
        try
        {
            Achievement? existingAchievement = await _context.Achievements.FindAsync(id);

            if (existingAchievement is null) return null;

            existingAchievement.DriverId = achievement.DriverId;
            existingAchievement.PolePosition = achievement.PolePosition;
            existingAchievement.RaceWins = achievement.RaceWins;
            existingAchievement.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingAchievement;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }
}