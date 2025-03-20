using Presentation.Core.Boundary.ValueObjects;

namespace Presentation.Application.API.Hubs
{
    public interface IPresentationHubClient
    {
        Task PushEnvelope(Envelope envelope);
    }
}
