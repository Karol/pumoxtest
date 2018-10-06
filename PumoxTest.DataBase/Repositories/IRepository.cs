using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PumoxTest.DataBase.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Get

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params string[] includeProperties);

        IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> filter = null,
            params string[] includeProperties);

        IEnumerable<TEntity> GetInclude(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params string[] includeProperties);

        #endregion

        #region GetRange

        IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int skipElementCount, int rangeElement,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetRange(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int skipElementCount, int rangeElement,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetRangeInclude(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int skipElementCount, int rangeElement,
            params string[] includeProperties);

        IEnumerable<TEntity> GetRangeInclude(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            int skipElementCount, int rangeElement,
            params string[] includeProperties);

        #endregion

        #region GetFirstOrDefault

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
           params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetFirstOrDefault(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetFirstOrDefaultInclude(Expression<Func<TEntity, bool>> filter,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
           params string[] includeProperties);

        TEntity GetFirstOrDefaultInclude(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params string[] includeProperties);

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
