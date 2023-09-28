using formulaOne.Entities.DbSets;

namespace formulaOne.DataService.Repositories.Interfaces;

public interface IDriverRepository
{
    Task<IEnumerable<Driver>> GetAllDriversAsync();

    Task<Driver?> GetDriverByIdAsync(Guid id);

    Task<Driver> AddDriverAsync(Driver driver);

    Task<Driver?> UpdateDriverAsync(Guid id, Driver driver);

    Task<bool> DeleteDriverByIdAsync(Guid id);
}