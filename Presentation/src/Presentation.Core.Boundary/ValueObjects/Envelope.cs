using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Presentation.Core.Boundary.ValueObjects
{
    public class Envelope : ValueObject
    {
        public Envelope(string eventType, string json)
        {
            EventType = eventType;
            Json = json;
        }

        public string EventType { get; private set; }
        public string Json { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return [
                EventType,
                Json,
            ];
        }
    }

    public class Envelope<T> : Envelope
    {
        public Envelope(T payload)
            : base(typeof(T).Name, JsonConvert.SerializeObject(payload))
        {
            Payload = payload;
        }

        public T Payload { get; private set; }
    }
}
