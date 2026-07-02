using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Application.Specifications
{
    internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }
        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            Take = pageSize;
            Skip = pageSize * (pageIndex - 1);
            IsPaginated = true;
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected BaseSpecifications(Expression<Func<TEntity, bool>> condition)
        {
            Criteria = condition;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> Includes)
        {
            IncludeExpressions.Add(Includes);
        }
    }
}
