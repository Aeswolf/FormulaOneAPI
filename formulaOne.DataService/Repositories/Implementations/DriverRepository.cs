using System.Data.Common;
using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using formulaOne.Entities.DbSets;
using Microsoft.EntityFrameworkCore;

namespace formulaOne.DataService.Repositories.Implementations;

public class DriverRepository : IDriverRepository
{
    private readonly APIDbContext _context;

    public DriverRepository(APIDbContext context)
    {
        _context = context;
    }

    public async Task<Driver> AddDriverAsync(Driver driver)
    {
        driver.Id = Guid.NewGuid();
        driver.CreatedAt = DateTime.UtcNow;
        driver.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return driver;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<bool> DeleteDriverByIdAsync(Guid id)
    {
        try
        {
            Driver? existingDriver = await _context.Drivers.FindAsync(id);

            if (existingDriver is null) return false;

            existingDriver.IsDeleted = true;
            existingDriver.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Driver>> GetAllDriversAsync()
    {
        try
        {
            return await _context.Drivers.ToListAsync();
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<Driver?> GetDriverByIdAsync(Guid id)
    {
        try
        {
            Driver? driverWithId = await _context.Drivers.FirstOrDefaultAsync(d => d.Id == id);

            return driverWithId;
        }
        catch (DbException)
        {
            throw;
        }
    }

    public async Task<Driver?> UpdateDriverAsync(Guid id, Driver driver)
    {
        try
        {
            Driver? existingDriver = await _context.Drivers.FindAsync(id);

            if (existingDriver is null) return null;

            existingDriver.FirstName = driver.FirstName;
            existingDriver.LastName = driver.LastName;
            existingDriver.DriverNumber = driver.DriverNumber;
            existingDriver.DateOfBirth = driver.DateOfBirth;
            existingDriver.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingDriver;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }
}