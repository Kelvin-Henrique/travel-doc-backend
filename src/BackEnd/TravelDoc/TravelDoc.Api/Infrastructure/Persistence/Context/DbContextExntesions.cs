using System.Data;
using Microsoft.EntityFrameworkCore;
using TravelDoc.Infrastructure.Core.Results;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Infrastructure.Persistence.Context
{
    internal static class DbContextExntesions
    {
        internal static async Task<Result> InTransactionAsync(
          this TravelDocDbContext db,
          Func<Task<Result>> work,
          IsolationLevel isolation = IsolationLevel.ReadCommitted,
          CancellationToken ct = default)
        {
            var strategy = db.Database.CreateExecutionStrategy();

            async Task<Result> Body()
            {
                await using var tx = await db.Database.BeginTransactionAsync(isolation, ct);
                var result = await work();
                if (result.IsFailure)
                {
                    await tx.RollbackAsync(ct);
                    return result;
                }

                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);

                return result;
            }

            return strategy.RetriesOnFailure
                ? await strategy.ExecuteAsync(Body)
                : await Body();
        }

        internal static async Task<Result<T>> InTransactionAsync<T>(
          this TravelDocDbContext db,
          Func<Task<Result<T>>> work,
          IsolationLevel isolation = IsolationLevel.ReadCommitted,
          CancellationToken ct = default) where T : class
        {
            var strategy = db.Database.CreateExecutionStrategy();
            async Task<Result<T>> Body()
            {
                await using var tx = await db.Database.BeginTransactionAsync(isolation, ct);
                var result = await work();
                if (result.IsFailure)
                {
                    await tx.RollbackAsync(ct);
                    return result;
                }

                await db.SaveChangesAsync(ct);
                await tx.CommitAsync(ct);
                return result;
            }
            return strategy.RetriesOnFailure
                ? await strategy.ExecuteAsync(Body)
                : await Body();
        }
    }
}
