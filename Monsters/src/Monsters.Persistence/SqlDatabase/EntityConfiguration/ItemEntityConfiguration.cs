using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Domain;

namespace Monsters.Persistence.SqlDatabase.EntityConfiguration
{
    public class ItemEntityConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder
                .Property(item => item.Name)
                .HasConversion(name => name.Value, dbName => new ItemName(dbName));

            builder.OwnsOne(item => item.AttributeSet);
        }
    }
}
