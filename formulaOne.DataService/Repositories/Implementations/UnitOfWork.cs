using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace formulaOne.DataService.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private APIDbContext _context;

    private ILogger _logger;
    public IDriverRepository driverRepository { get; }

    public IAchievementRepository achievementRepository { get; }

    public UnitOfWork(APIDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;

        driverRepository = new DriverRepository(_context, logger);
        achievementRepository = new AchievementRepository(_context, logger);
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _logger.Error("Error occurred while saving data to the database");
            throw;
        }
    }
}