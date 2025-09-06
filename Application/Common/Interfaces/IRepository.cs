using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(CancellationToken cancellationToken = default);
    Task<List<T>> ListAsync(Func<IQueryable<T>, IQueryable<T>> queryShaper, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}