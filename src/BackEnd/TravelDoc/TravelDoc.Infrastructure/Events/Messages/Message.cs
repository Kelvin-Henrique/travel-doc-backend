using System;

namespace TravelDoc.Infrastructure.Core.Events.Messages
{
    public abstract class Message
    {
        protected Message()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.Now;
        }
        public Guid Id { get; protected set; }
        public DateTime CreationDate { get; protected set; }
    }
}
