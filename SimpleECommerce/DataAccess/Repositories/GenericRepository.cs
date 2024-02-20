using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SimpleECommerce.Core.Interfaces;
using SimpleECommerce.Core.Consts;
using System.Linq.Dynamic.Core;

namespace SimpleECommerce.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected SimpleECommerceDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(SimpleECommerceDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAllIQueryable()
        {
            return _dbSet;
        }



        // Specific method for inclusion
        public async Task<List<T>> GetWithIncludes(Expression<Func<T, object>> includePath)
        {
            return await _context.Set<T>().Include(includePath).ToListAsync();
        }
        public IQueryable<TProjection> GetCustomColumns<TProjection>(Expression<Func<T, bool>> filter,
                                                         Expression<Func<T, TProjection>> projection)
        {
            return _context.Set<T>()
                          .Where(filter)
                          .Select(projection);
        }




        public async Task<(int recordsCount, List<T> results)> FindAllAsyncCustom(
  Expression<Func<T, bool>>? criteria, int? skip, int? take,
  string sortColumn, string sortColumnDirection, bool tracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (criteria != null)
                query = query.Where(criteria);

            if (sortColumn != null)
                query = query.OrderBy(sortColumn + " " + sortColumnDirection);

            int recordsCount = await query.CountAsync();

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);


            if (tracking != true)
                query = query.AsNoTracking();



            var results = await query.ToListAsync();



            return (recordsCount, results);
        }


        public async Task<(int recordsCount, List<T> results)> FindAllAsyncCustom(
   Expression<Func<T, bool>>? criteria, int? skip, int? take,
   string sortColumn, string sortColumnDirection, Expression<Func<T, object>> includePath, bool tracking = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includePath != null)
                query = query.Include(includePath);

            if (criteria != null)
                query = query.Where(criteria);

            if (sortColumn != null)
                query = query.OrderBy(sortColumn + " " + sortColumnDirection);

            int recordsCount = await query.CountAsync();

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);


            if (tracking != true)
                query = query.AsNoTracking();



            var results = await query.ToListAsync();



            return (recordsCount, results);
        }


        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }


        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        //public virtual  T  GetByIdAsync(Expression<Func<T, bool>> criteria)
        //{
        //    return   _dbSet.FirstOrDefault(criteria);
        //}

        public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return _dbSet.Where(criteria).Skip(skip).Take(take).ToList();
        }

        //.asNoTraking
        //    groupBy
        //    Includes
        //    thenOrderby
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _dbSet.Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip)
        {
            return await _dbSet.Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _dbSet.Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public T Add(T entity)
        {
            _dbSet.Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return entities;
        }

        //public  Task Update(T entity)
        //{
        //    // In case AsNoTracking is used
        //    _context.Entry(entity).State = EntityState.Modified;
        //    return _context.SaveChangesAsync();
        //}

        //public Task Remove(T entity)
        //{
        //    _context.Set<T>().Remove(entity);
        //    return _context.SaveChangesAsync();
        //}

        public T Update(T entity)
        {
            _context.Update(entity);
            return entity;
        }

        public T UpdateAttach(T entity)
        {
            _context.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _dbSet.AttachRange(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _dbSet.Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _dbSet.CountAsync(criteria);
        }


        public bool IsExist(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

    }
}
