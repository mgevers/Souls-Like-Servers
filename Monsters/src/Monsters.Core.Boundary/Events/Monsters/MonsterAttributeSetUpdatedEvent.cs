using Common.Core.Boundary;

namespace Monsters.Core.Boundary.Events.Monsters
{
    public class MonsterAttributeSetUpdatedEvent
    {
        public MonsterAttributeSetUpdatedEvent(
            Guid monsterId,
            SoulsAttributeSet attributeSet)
        {
            MonsterId = monsterId;
            AttributeSet = attributeSet;
        }

        public Guid MonsterId { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
