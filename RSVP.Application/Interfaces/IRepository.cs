using System;

namespace RSVP.Application.Interfaces;

public interface IRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);
    void Delete(T entity);
    Task SaveChangesAsync();
}
