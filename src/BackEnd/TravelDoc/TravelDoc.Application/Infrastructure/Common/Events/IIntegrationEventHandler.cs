using System.Threading;
using System.Threading.Tasks;

namespace TravelDoc.Infrastructure.Core.Events
{
    public interface IIntegrationEventHandler<TEvent> where TEvent : IntegrationEvent
    {
        Task Handle(TEvent notification, CancellationToken cancellationToken);
    }
}
