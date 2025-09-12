using MediatR;
using Microsoft.Extensions.Logging;
using TravelDoc.Infrastructure.Core.Domain;
using TravelDoc.Infrastructure.Core.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TravelDoc.Infrastructure.Core.Events
{
    public abstract class IntegrationEventHandler<TEvent>
    {
        private readonly DomainNotificationHandler _notification;
        protected readonly IMediator _mediator;
        protected readonly ILogger<TEvent> _logger;

        protected IntegrationEventHandler(INotificationHandler<DomainNotification> notification, IMediator mediator, ILogger<TEvent> logger)
        {
            _notification = (DomainNotificationHandler)notification;
            _mediator = mediator;
            _logger = logger;
        }

        public async ValueTask SendAsync(IBaseRequest request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);

            if (!_notification.HasNotifications())
            {
                return;
            }

            _notification.GetNotifications()
                .ForEach(error =>
                {
                    _logger.LogError($"Error: {error.Key} Message: {error.Value}", GetType().Name);
                });
        }
        public async ValueTask PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : Event
        {
            await _mediator.Publish(@event, cancellationToken);
        }

        public async ValueTask PublishAsync<T>(IEnumerable<T> events, CancellationToken cancellationToken = default) where T : Event
        {
            foreach (var @event in events)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }

        public async ValueTask PublishAsync(Entity entity, CancellationToken cancellationToken = default)
        {
            await PublishAsync(entity.Events, cancellationToken);
        }

        public async ValueTask PublishAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default)
        {
            await PublishAsync(entities.SelectMany(e => e.Events), cancellationToken);
        }

        public async ValueTask PublishAsync(params Entity[] entities)
        {
            await PublishAsync(entities.ToList());
        }
    }
}
