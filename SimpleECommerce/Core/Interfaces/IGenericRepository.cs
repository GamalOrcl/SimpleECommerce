using SimpleECommerce.Core.Consts;
using SimpleECommerce.Core.Entities;
using System.Linq.Expressions;

namespace SimpleECommerce.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {

        Task<List<T>> GetWithIncludes(Expression<Func<T, object>> includePath);
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAllIQueryable();

        // Task<(int recordsCount, IQueryable<TDestination>)> CustomQuery<TDestination>(
        //Expression<Func<T, bool>>? criteria, int? skip, int? take,
        //string sortColumn, string sortDirection, bool tracking = false);

        Task<(int recordsCount, List<T> results)> FindAllAsyncCustom(
   Expression<Func<T, bool>>? criteria, int? skip, int? take,
   string sortColumn, string sortColumnDirection, bool Tracking = false);

        Task<(int recordsCount, List<T> results)> FindAllAsyncCustom(
   Expression<Func<T, bool>>? criteria, int? skip, int? take,
   string sortColumn, string sortColumnDirection, Expression<Func<T, object>> includePath, bool tracking = false);
        Task<List<T>> GetAllAsync();
        T Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        T UpdateAttach(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);

        bool IsExist(Expression<Func<T, bool>> predicate = null);




    }
}
