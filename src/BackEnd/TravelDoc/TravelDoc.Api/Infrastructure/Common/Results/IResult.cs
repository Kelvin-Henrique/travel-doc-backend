namespace TravelDoc.Infrastructure.Core.Results
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }

    public interface IResult<out T, out E> : IResult, IValue<T>, IError<E>
    {
    }
}
