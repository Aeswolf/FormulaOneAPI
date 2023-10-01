using formulaOne.DataService.Context;
using formulaOne.DataService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace formulaOne.DataService.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected ILogger _logger;
    internal DbSet<T> _dbSet;

    public GenericRepository(APIDbContext context, ILogger logger)
    {
        _logger = logger;
        _dbSet = context.Set<T>();
    }

    public virtual Task<T> AddAsync(T obj)
    {
        throw new NotImplementedException();
    }

    public virtual Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<IEnumerable<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public virtual Task<T?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public virtual Task<T?> UpdateAsync(Guid id, T obj)
    {
        throw new NotImplementedException();
    }
}