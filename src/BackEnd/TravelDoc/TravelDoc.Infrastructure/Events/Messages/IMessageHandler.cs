using TravelDoc.Infrastructure.Core.Results;
using System.Threading;
using System.Threading.Tasks;

namespace TravelDoc.Infrastructure.Core.Events.Messages
{
    public interface IMessageHandler<TMessage> where TMessage : Message
    {
        Task<Result> Handle(TMessage message, CancellationToken cancellationToken);
    }
}
