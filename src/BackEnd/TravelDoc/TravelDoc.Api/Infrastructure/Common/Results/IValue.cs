namespace TravelDoc.Infrastructure
{
    public interface IValue<out T>
    {
        T Value { get; }
    }
}
