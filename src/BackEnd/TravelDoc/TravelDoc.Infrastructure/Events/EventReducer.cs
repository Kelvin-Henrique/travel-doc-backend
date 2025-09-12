using TravelDoc.Infrastructure.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace TravelDoc.Infrastructure.Core.Events
{
    public class EventReducer
    {
        public static IEnumerable<Event> ReduceEvents(Entity entity)
        {
            if (entity is null || !(entity.Events?.Any() ?? false))
            {
                return Enumerable.Empty<Event>();
            }

            var reducedEvents = new List<Event>();

            foreach (var @event in entity.Events)
            {
                if (entity.Events.Any(existing => existing.IsSupersetFor(@event)))
                {
                    continue;
                }

                reducedEvents.Add(@event);
            }

            return reducedEvents;
        }
    }
}
