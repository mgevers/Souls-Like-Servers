using Common.Core.Boundary;

namespace Monsters.Core.Commands.Monsters
{
    public class UpdateMonsterAttributeSetCommand
    {
        public UpdateMonsterAttributeSetCommand(
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
