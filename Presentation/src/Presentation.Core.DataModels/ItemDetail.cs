using Common.Core.Boundary;
using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;

namespace Presentation.Core.DataModels
{
    public class ItemDetail : Entity<Guid>, IDataModel
    {
        public ItemDetail(
            Guid id,
            string itemName,
            SoulsAttributeSet attributeSet) : base(id)
        {
            ItemName = itemName;
            AttributeSet = attributeSet;
        }

        public string ItemName { get; set; } = null!;

        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }
}
