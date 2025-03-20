using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Domain;

namespace Monsters.Persistence.SqlDatabase.EntityConfiguration
{
    public class MonsterEntityConfiguration : IEntityTypeConfiguration<Monster>
    {
        public void Configure(EntityTypeBuilder<Monster> builder)
        {
            builder.OwnsOne(monster => monster.AttributeSet);

            builder
                .Property(monster => monster.Name)
                .HasConversion(name => name.Value, dbName => new MonsterName(dbName));

            builder
                .Property(monster => monster.Level)
                .HasConversion(level => level.Value, dbLevel => new MonsterLevel(dbLevel));
        }
    }
}
