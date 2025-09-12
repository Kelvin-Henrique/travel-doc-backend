using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Infrastructure.Persistence.Context
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly TravelDocDbContext _context;

        public UnitOfWork(TravelDocDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Result> RunTransactionAsync(Func<Task<Result>> task, CancellationToken cancellationToken = default)
        {
            return await _context.InTransactionAsync(task, System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }

        public async Task<Result<T>> RunTransactionAsync<T>(Func<Task<Result<T>>> task, CancellationToken cancellationToken = default) where T : class
        {
            return await _context.InTransactionAsync(task, System.Data.IsolationLevel.ReadCommitted, cancellationToken);
        }
    }
}
