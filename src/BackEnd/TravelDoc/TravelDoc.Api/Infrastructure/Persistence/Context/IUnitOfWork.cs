using TravelDoc.Infrastructure.Core.Results;

namespace TravelDoc.Infrastructure.Persistence.Context
{
    public interface IUnitOfWork
    {
        Task<Result> RunTransactionAsync(Func<Task<Result>> task, CancellationToken cancellationToken = default);
        Task<Result<T>> RunTransactionAsync<T>(Func<Task<Result<T>>> task, CancellationToken cancellationToken = default) where T : class;
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
