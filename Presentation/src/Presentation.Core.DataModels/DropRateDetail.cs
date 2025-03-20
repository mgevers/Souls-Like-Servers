using Common.Infrastructure.Persistence;
using CSharpFunctionalExtensions;

namespace Presentation.Core.DataModels
{
    public class DropRateDetail : Entity<Guid>, IDataModel
    {
        public DropRateDetail(
            Guid id,
            ItemDetail item,
            double dropRate) : base(id)
        {
            Item = item;
            DropRate = dropRate;
        }

        public ItemDetail Item { get; set; } = null!;

        public double DropRate { get; set; }
    }
}
