using SimpleECommerce.Core.Interfaces;
using SimpleECommerce.DataAccess.Repositories;
using System.Collections;

namespace SimpleECommerce.DataAccess.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SimpleECommerceDbContext _context;
        private Hashtable _repositories;

        //public IStudentsRepository Students { get; private set; }


        public UnitOfWork(SimpleECommerceDbContext context)
        {
            _context = context;

            //Students = new StudentsRepository(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
