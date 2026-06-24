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
            //var typeName = typeof(TEntity).Name;
            //if (_repositories.TryGetValue(typeName, out var repository))
            //    return (IRepository<TEntity, TKey>)repository;
            //else
            //{
            //    var repo = new Repository<TEntity, TKey>(dbContext);
            //    _repositories[typeName] = repo;
            //    return repo;
            //}
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repository))
            {
                repository = new Repository<TEntity, TKey>(dbContext);
                _repositories.Add(entityType, repository);
            }

            return (IRepository<TEntity, TKey>)repository;
        }

        //public IRepository<Product, int> Products => throw new NotImplementedException();

        //public IRepository<ProductType, int> ProductType => throw new NotImplementedException();

        //public IRepository<ProductBrand, int> ProductBrands => throw new NotImplementedException();


        public Task<int> SaveChangesAsunc(CancellationToken ct)
            => dbContext.SaveChangesAsync(ct);

    }
}
