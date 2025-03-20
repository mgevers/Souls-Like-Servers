using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Domain;

namespace Monsters.Persistence.SqlDatabase.EntityConfiguration
{
    public class DropTableEntityConfiguration : IEntityTypeConfiguration<DropTable>
    {
        public void Configure(EntityTypeBuilder<DropTable> builder)
        {
            builder.HasMany(dropTable => dropTable.Rows);

            builder
                .Property(dropTable => dropTable.RollCount)
                .HasConversion(rollCount => rollCount.Value, dbRollCount => new RollCount(dbRollCount));
        }
    }

    public class DropTableRowConfiguration : IEntityTypeConfiguration<DropTableRow>
    {
        public void Configure(EntityTypeBuilder<DropTableRow> builder)
        {
            builder
                .Property(entry => entry.DropRateDenominator)
                .HasConversion(dropRate => dropRate.Value, dbDropRate => new DropRateDenominator(dbDropRate));
        }
    }
}
