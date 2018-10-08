using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PumoxTest.DataBase.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Get

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Get(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        #endregion

        #region GetFirstOrDefault

        TEntity GetFirstOrDefault(
           Expression<Func<TEntity, bool>> filter,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        TEntity GetFirstOrDefault(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);

        TEntity GetFirstOrDefaultInclude(
           Expression<Func<TEntity, bool>> filter,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
           params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetFirstOrDefaultInclude(
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
           params Expression<Func<TEntity, object>>[] includeProperties);

        #endregion

        TEntity GetById(object id);

        void Insert(TEntity entity);

        void Insert(IEnumerable<TEntity> entitys);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

        void Update(IEnumerable<TEntity> entitysToUpdate);
    }
}
