using System.Threading.Tasks;
using System.Data;
namespace DDD.DomainLayer
{
    public interface IUnitOfWork
    {
        Task<bool> SaveEntitiesAsync();
        Task StartAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitAsync();
        Task RollbackAsync();
    }
}
