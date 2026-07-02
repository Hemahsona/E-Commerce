using E_Commerce.Domain.Contract.IRepositories;
using E_Commerce.Domain.Contract.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data.Contect;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class UnitOfWork(StoreDBContext dbContext) : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = [];
        public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repository))
            {
                repository = new Repository<TEntity, TKey>(dbContext);
                _repositories.Add(entityType, repository);
            }

            return (IRepository<TEntity, TKey>)repository;
        }
        public Task<int> SaveChangesAsunc(CancellationToken ct)
            => dbContext.SaveChangesAsync(ct);

    }
}
