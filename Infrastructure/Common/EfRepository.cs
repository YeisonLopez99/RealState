using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public class EfRepository<T> : IRepository<T> where T : class
{
    private readonly Infrastructure.Persistence.ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;
    public EfRepository(Infrastructure.Persistence.ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>  _dbSet.Add(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => _dbSet.FindAsync(new object[] { id }, cancellationToken).AsTask();
    public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default) => await _dbSet.ToListAsync(cancellationToken);
    public async Task<List<T>> ListAsync(
    Func<IQueryable<T>, IQueryable<T>>? include = null,
    CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (include != null)
            query = include(query);

        return await query.ToListAsync(cancellationToken);
    }
    public void Update(T entity) => _dbSet.Update(entity);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
}