using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contract.Repositories
{
    public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default);
        Task<int> CountAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default);
    }
}
