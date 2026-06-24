using E_Commerce.Domain.Contract.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Contect;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class Repository<TEntity, TKey>(StoreDBContext dbContext) : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
            => await _dbSet.FindAsync(id, ct);

        public async Task AddAsync(TEntity entity, CancellationToken ct = default)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }
    }
}
