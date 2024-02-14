using Domain;
using Domain.Entities;
using Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ICurrentUser _currentUser;
		private readonly ApplicationDbContext _context;
		public UnitOfWork(ApplicationDbContext context, ICurrentUser currentUser)
		{
			_context = context;
			_currentUser = currentUser;
		}

		public IRepository<T> Repository<T>() where T : Entity
		{
			return new Repository<T>(_context, _currentUser);
		}

		public bool Commit()
		{
			return _context.SaveChanges()>0;
		}
		public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
		{
			return await _context.SaveChangesAsync(cancellationToken)>0;
		}

	}
}
