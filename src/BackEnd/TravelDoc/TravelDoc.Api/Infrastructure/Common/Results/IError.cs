namespace TravelDoc.Infrastructure.Core.Results
{
    public interface IError<out E>
    {
        E ErrorValue { get; }
    }
}
