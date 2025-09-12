using TravelDoc.Infrastructure.Core.Events.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelDoc.Infrastructure.Core.Events
{
    public class Event : Message, INotification
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }

        protected virtual IEnumerable<Type> SupersetFor => null;

        public bool IsSupersetFor(Event domainEvent)
        {
            return domainEvent != null && SupersetFor != null && SupersetFor!.Any(type => type == domainEvent.GetType());
        }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
