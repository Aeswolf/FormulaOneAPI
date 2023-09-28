namespace formulaOne.DataService.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    Task<T> AddAsync(T obj);

    Task<bool> DeleteByIdAsync(Guid id);

    Task<T?> UpdateAsync(Guid id, T obj);
}