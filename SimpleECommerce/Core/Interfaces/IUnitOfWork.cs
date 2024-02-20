namespace SimpleECommerce.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        //IStudentsRepository Students { get; }

        Task<int> Complete();
    }
}
