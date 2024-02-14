using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        bool Commit();
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);

        IRepository<TEntity> Repository<TEntity>() where TEntity : Entity;
    }
}
