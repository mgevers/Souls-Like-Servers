using Ardalis.Result;

namespace Monsters.Core.Boundary.Events
{
    public class FailureEvent
    {
        public FailureEvent(
            ResultStatus status,
            IReadOnlyCollection<string> errors,
            string? connectionId = null)
        {
            Status = status;
            Errors = [.. errors];
            ConnectionId = connectionId;
        }

        public ResultStatus Status { get; private set; }
        public IReadOnlyList<string> Errors { get; private set; }
        public string? ConnectionId { get; private set; }
    }
}
