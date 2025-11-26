using System;

namespace RSVP.Application.Interfaces;

public interface IRepository<T> where T: class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    void Delete(T entity);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
