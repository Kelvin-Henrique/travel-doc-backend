using TravelDoc.Infrastructure.Core.Events;
using System.Collections.Generic;

namespace TravelDoc.Infrastructure.Core.Domain
{
    public abstract class Entity
    {
        public int Id { get; protected set; }

        protected Entity()
        {
            _events = new List<Event>();
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }

        public bool ExisteId()
        {
            return Id > 0;
        }

        public virtual void IncluirId(int id)
        {
            Id = id;
        }

        protected List<Event> _events;

        public IReadOnlyList<Event> Events => _events?.AsReadOnly() ?? new List<Event>().AsReadOnly();

        public void ClearDomainEvents()
        {
            _events?.Clear();
        }
    }
}
