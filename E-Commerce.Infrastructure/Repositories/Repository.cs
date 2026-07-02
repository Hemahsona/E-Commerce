using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Contract.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data.Contect;
using E_Commerce.Infrastructure.Specifications;
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

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            //IQueryable<TEntity> query = dbContext.Set<TEntity>().AsNoTracking();
            //if (spec != null)
            //{
            //    if (spec.IncludeExpressions.Any())
            //    {
            //        foreach (var expression in spec.IncludeExpressions)
            //            query = query.Include(expression);
            //    }
            //}
            var query = SpecificationsEvaluator.CreateQuery(_dbSet, spec);
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            var query = SpecificationsEvaluator.CreateQuery(_dbSet, spec);
            return await query.FirstOrDefaultAsync(ct);

        }

        public async Task<int> CountAsync(ISpecifications<TEntity, TKey> spec, CancellationToken ct = default)
        {
            return await SpecificationsEvaluator.CreateQuery(_dbSet, spec).CountAsync(ct);
        }
    }
}
