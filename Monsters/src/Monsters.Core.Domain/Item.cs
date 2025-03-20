using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;
using Monsters.Core.Boundary.ValueObjects;

namespace Monsters.Core.Domain
{
    public class Item : Entity<Guid>, IDataModel
    {
        public Item(
            Guid id,
            ItemName name,
            SoulsAttributeSet attributeSet) : base(id)
        {
            Name = name;
            AttributeSet = attributeSet;
        }

        private Item() { }

        public ItemName Name { get; set; } = null!;
        
        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }
}
