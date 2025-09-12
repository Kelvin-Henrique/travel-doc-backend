using TravelDoc.Infrastructure.Core.Notifications;
using TravelDoc.Infrastructure.Core.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TravelDoc.Infrastructure.Core.Events.Messages
{
    public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : Message
    {
        private readonly DomainNotificationHandler _notification;
        protected readonly IMediator _mediator;
        protected readonly ILogger<TMessage> _logger;

        protected MessageHandler(INotificationHandler<DomainNotification> notification, IMediator mediator, ILogger<TMessage> logger)
        {
            _notification = (DomainNotificationHandler)notification;
            _mediator = mediator;
            _logger = logger;
        }

        public abstract Task<Result> Handle(TMessage message, CancellationToken cancellationToken);

        public bool IsSuccess() => !_notification.HasNotifications();

        public bool IsFailure() => !IsSuccess();

        public Dictionary<string, string> GetErrors()
        {
            var items = _notification
                        .GetNotifications()
                        .Select(notification => new KeyValuePair<string, string>(notification.Key, notification.Value));

            return new Dictionary<string, string>(items);
        }
    }
}
