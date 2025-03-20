using Common.Core.Boundary;

namespace Monsters.Core.Commands.Items
{
    public class UpdateItemAttributeSetCommand
    {
        public UpdateItemAttributeSetCommand(
            Guid itemId,
            SoulsAttributeSet attributeSet)
        {
            ItemId = itemId;
            AttributeSet = attributeSet;
        }

        public Guid ItemId { get; private set; }
        public SoulsAttributeSet AttributeSet { get; private set; }
    }
}
