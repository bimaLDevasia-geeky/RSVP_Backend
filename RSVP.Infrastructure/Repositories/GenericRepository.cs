using System;
using Microsoft.EntityFrameworkCore;
using RSVP.Application.Interfaces;
using RSVP.Infrastructure.Persistence;

namespace RSVP.Infrastructure.Repositories;

public class GenericRepository<T> : IRepository<T> where T : class
{
    private readonly RsvpDbContext _context;

    public GenericRepository(RsvpDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
    }

    public async Task AddAsync(T entity)
    {
        await  _context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
