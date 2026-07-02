using E_Commerce.Domain.Contract.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Domain.Contract.IRepositories
{
    public interface IUnitOfWork
    {
        IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<int> SaveChangesAsunc(CancellationToken ct);
    }
}
