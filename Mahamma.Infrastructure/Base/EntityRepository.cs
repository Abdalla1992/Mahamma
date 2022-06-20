using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Base
{
    public abstract class EntityRepository<T> where T : class
    {
        #region Vars
        protected Context.MahammaContext AppDbContext { get; }
        protected IMapper Mapper { get; }
        #endregion

        #region INI
        internal EntityRepository(Context.MahammaContext appDbContext, IMapper mapper)
        {
            AppDbContext = appDbContext;
            AppDbContext = appDbContext ?? throw new ArgumentNullException(nameof(AppDbContext));
            Mapper = mapper;
        }
        #endregion

        #region Get Page
        public async Task<List<T>> GetPageAsync<TKey>(int skipCount, int takeCount, Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> sortingExpression, int sortDir, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();
            skipCount *= takeCount;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            switch (sortDir)
            {
                case 0:
                    if (skipCount == 0)
                        query = query.OrderBy<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderBy<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                case 1:
                    if (skipCount == 0)
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                default:
                    break;
            }
            return await query.AsNoTracking().ToListAsync();

        }
        public async Task<List<T>> GetPageAsyncWithoutQueryFilter<TKey>(int skipCount, int takeCount, Expression<Func<T, bool>> filter, Expression<Func<T, TKey>> sortingExpression, int sortDir, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();
            skipCount *= takeCount;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).IgnoreQueryFilters();

            switch (sortDir)
            {
                case 0:
                    if (skipCount == 0)
                        query = query.OrderBy<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderBy<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                case 1:
                    if (skipCount == 0)
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Take(takeCount);
                    else
                        query = query.OrderByDescending<T, TKey>(sortingExpression).Skip(skipCount).Take(takeCount);
                    break;
                default:
                    break;
            }

            return await query.AsNoTracking().ToListAsync();
        }
        #endregion

        #region GetAll
        public async Task<List<T>> GetAllAsync()
        {
            return await AppDbContext.Set<T>().AsNoTracking().ToListAsync();
        }
        #endregion

        #region First Or Default
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultAsyncWithoutQueryFilter(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter).IgnoreQueryFilters();
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultNoTrackingAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<T> FirstOrDefaultNoTrackingAsyncWithoutQueryFilter(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter).IgnoreQueryFilters();
            }
            var result = await query.AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Get Where
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query;
        }
        public IQueryable<T> GetWhereNoTracking(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query.AsNoTracking();
        }
        public IQueryable<T> GetWhereWithoutQueryFilter(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter).IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query;
        }
        public IQueryable<T> GetWhereNoTrackingAsyncWithoutQueryFilter(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter).IgnoreQueryFilters();
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            return query.AsNoTracking();
        }

        public async Task<List<T>> GetWhereAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetWhereNoTrackingAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<List<T>> GetWhereWithoutQueryFilterAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.IgnoreQueryFilters().ToListAsync();
        }
        public async Task<List<T>> GetWhereAsynctNoTrackingAsyncWithoutQueryFilter(Expression<Func<T, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<T> query = AppDbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter).IgnoreQueryFilters();
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.AsNoTracking().ToListAsync();
        }

        #endregion

        #region Get Any
        public async Task<bool> GetAnyAsync(Expression<Func<T, bool>> filter = null)
        {
            return await AppDbContext.Set<T>().AnyAsync(filter);
        }
        #endregion

        #region Get Count
        public async Task<int> GetCountAsync()
        {
            return await AppDbContext.Set<T>().CountAsync();
        }
        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter)
        {
            return await AppDbContext.Set<T>().Where(filter).CountAsync();
        }
        public async Task<int> GetCountAsyncWithoutQueryFilter(Expression<Func<T, bool>> filter)
        {
            return await AppDbContext.Set<T>().Where(filter).IgnoreQueryFilters().CountAsync();
        }
        #endregion

        #region Create 
        public void CreateAsyn(T entity)
        {
            AppDbContext.Set<T>().AddAsync(entity);
        }
        public void CreateListAsyn(List<T> entityList)
        {
            AppDbContext.Set<T>().AddRangeAsync(entityList);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            AppDbContext.Set<T>().Update(entity);
        }
        public void UpdateList(List<T> entityList)
        {
            AppDbContext.Set<T>().UpdateRange(entityList);
        }
        #endregion

        #region Delete
        public void Delete(T entity)
        {
            AppDbContext.Set<T>().Remove(entity);
        }
        public void DeleteList(List<T> entityList)
        {
            AppDbContext.Set<T>().RemoveRange(entityList);
        }
        #endregion
    }
}
