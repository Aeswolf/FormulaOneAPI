using System.Data.Common;
using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace formulaOne.DataService.Repositories.Implementations;

public class DriverRepository : GenericRepository<Driver>, IDriverRepository
{

    public DriverRepository(APIDbContext context, ILogger logger) : base(context, logger) { }
    public override async Task<Driver> AddAsync(Driver driver)
    {
        driver.Id = Guid.NewGuid();
        driver.CreatedAt = DateTime.UtcNow;
        driver.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _dbSet!.AddAsync(driver);
            return driver;
        }
        catch (DbUpdateException)
        {
            _logger.Error("Error occurred adding a new driver to the database");
            throw;
        }
    }

    public override async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            Driver? existingDriver = await _dbSet!.FirstOrDefaultAsync(d => d.IsDeleted == false && d.Id == id);

            if (existingDriver is null) return false;

            existingDriver.IsDeleted = true;
            existingDriver.DeletedAt = DateTime.UtcNow;

            return true;
        }
        catch (DbException)
        {
            _logger.Error("Error occurred when deleting a driver from the database");
            throw;
        }
    }

    public override async Task<IEnumerable<Driver>> GetAllAsync()
    {
        try
        {
            return await _dbSet!.Where(d => d.IsDeleted == false)
                        .AsNoTracking()
                        .AsSplitQuery()
                        .OrderBy(d => d.CreatedAt)
                        .ToListAsync();
        }
        catch (DbException)
        {
            _logger.Error("Error occurred retrieving drivers from the database");
            throw;
        }
    }

    public override async Task<Driver?> GetByIdAsync(Guid id)
    {
        try
        {
            Driver? driverWithId = await _dbSet!.FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);

            return driverWithId;
        }
        catch (DbException)
        {
            _logger.Error("Error occurred retrieving a driver from the database");
            throw;
        }
    }

    public override async Task<Driver?> UpdateAsync(Guid id, Driver driver)
    {
        try
        {
            Driver? existingDriver = await _dbSet!.FirstOrDefaultAsync(d => d.Id == id && d.IsDeleted == false);

            if (existingDriver is null) return null;

            existingDriver.FirstName = driver.FirstName;
            existingDriver.LastName = driver.LastName;
            existingDriver.DriverNumber = driver.DriverNumber;
            existingDriver.DateOfBirth = driver.DateOfBirth;
            existingDriver.UpdatedAt = DateTime.UtcNow;

            return existingDriver;
        }
        catch (DbUpdateException)
        {
            _logger.Error("Error occurred while updating a driver instance in the database");
            throw;
        }
    }
}