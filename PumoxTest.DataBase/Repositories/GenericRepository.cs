using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PumoxTest.DataBase.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext Context;
        protected DbSet<TEntity> DbSet;

        public GenericRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        #region Get
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = CreateQuery(filter, orderBy);
            foreach (var include in includeProperties)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Get(filter, null, includeProperties);
        }

        public IEnumerable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Get(null, orderBy, includeProperties);
        }

        public virtual IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            IQueryable<TEntity> query = CreateQuery(filter, orderBy);

            foreach (var include in includeProperties)
            {
                query = query.Include(include.Trim());
            }
            return query.ToList();
        }

        public IEnumerable<TEntity> GetInclude(Expression<Func<TEntity, bool>> filter = null, params string[] includeProperties)
        {
            return GetInclude(filter, null, includeProperties);
        }

        public IEnumerable<TEntity> GetInclude(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] includeProperties)
        {
            return GetInclude(null, orderBy, includeProperties);
        }

        public IEnumerable<TEntity> GetInclude(params string[] includeProperties)
        {
            return GetInclude(null, null, includeProperties);
        }

        #endregion        

        #region GetFirstOrDefault

        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = CreateQuery(filter, orderBy);
            foreach (var include in includeProperties)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault();
        }

        public TEntity GetFirstOrDefault(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetFirstOrDefault(orderBy, includeProperties);
        }

        public virtual TEntity GetFirstOrDefaultInclude(Expression<Func<TEntity, bool>> filter,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params string[] includeProperties)
        {
            IQueryable<TEntity> query = CreateQuery(filter, orderBy);
            foreach (var include in includeProperties)
            {
                query = query.Include(include.Trim());
            }
            return query.FirstOrDefault();
        }

        public TEntity GetFirstOrDefaultInclude(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params string[] includeProperties)
        {
            return GetFirstOrDefaultInclude(null, orderBy, includeProperties);
        }

        #endregion

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }


        private IQueryable<TEntity> CreateQuery(Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entitys)
        {
            DbSet.AddRange(entitys);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Update(IEnumerable<TEntity> entitysToUpdate)
        {
            foreach (var entity in entitysToUpdate)
            {
                Update(entity);
            }
        }
    }
}
