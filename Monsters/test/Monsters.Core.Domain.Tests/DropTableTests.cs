using Ardalis.Result;
using Common.Testing.Assert;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Testing;

namespace Monsters.Core.Domain.Tests
{
    public class DropTableTests
    {
        [Fact]
        public void CanCreateDropTable()
        {
            var tableId = Guid.NewGuid();
            var monster = Entities.CreateMonster();
            var rollCount = new RollCount(1);

            var dropTable = new DropTable(
                id: tableId,
                monster: monster,
                rollCount: rollCount,
                rows: []);

            Assert.Equal(tableId, dropTable.Id);
            AssertExtensions.DeepEqual(monster, dropTable.Monster);
            Assert.Equal(rollCount, dropTable.RollCount);
            Assert.Empty(dropTable.Rows);
        }

        [Fact]
        public void CanCreateDropTable_With100PercentDropRate()
        {
            var tableId = Guid.NewGuid();
            var monster = Entities.CreateMonster();
            var rollCount = new RollCount(1);

            IReadOnlyCollection<DropTableRow> rows = [
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
            ];

            var dropTable = new DropTable(
                id: tableId,
                monster: monster,
                rollCount: rollCount,
                rows: rows);

            Assert.Equal(tableId, dropTable.Id);
            AssertExtensions.DeepEqual(monster, dropTable.Monster);
            Assert.Equal(rollCount, dropTable.RollCount);
            AssertExtensions.DeepEqual(rows, dropTable.Rows);
        }

        [Fact]
        public void CreateDropTable_WithOver100PercentDropRate_ThrowsError()
        {
            var tableId = Guid.NewGuid();
            var monster = Entities.CreateMonster();
            var rollCount = new RollCount(1);
            IReadOnlyCollection<DropTableRow> rows = [
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
            ];

            Assert.ThrowsAny<Exception>(() =>
            {
                var dropTable = new DropTable(
                    id: tableId,
                    monster: monster,
                    rollCount: rollCount,
                    rows: rows);
            });
        }

        [Fact]
        public void AddRow_StillBelow100PercentDropRate_ReturnsSucess()
        {
            var tableId = Guid.NewGuid();
            var monster = Entities.CreateMonster();
            var rollCount = new RollCount(1);
            IReadOnlyCollection<DropTableRow> rows = [
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(4)),
            ];

            var dropTable = new DropTable(
                id: tableId,
                monster: monster,
                rollCount: rollCount,
                rows: rows);

            var newRow = new DropTableRow(Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(4));
            var addResult = dropTable.AddRow(newRow);

            Assert.True(addResult.IsSuccess);
            Assert.Equal(tableId, dropTable.Id);
            AssertExtensions.DeepEqual(monster, dropTable.Monster);
            Assert.Equal(rollCount, dropTable.RollCount);
            AssertExtensions.DeepEqual([.. rows, newRow], dropTable.Rows);
        }

        [Fact]
        public void AddRow_Over100PercentDropRate_ReturnsFailure()
        {
            var tableId = Guid.NewGuid();
            var monster = Entities.CreateMonster();
            var rollCount = new RollCount(1);
            IReadOnlyCollection<DropTableRow> rows = [
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
                new (Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(2)),
            ];

            var dropTable = new DropTable(
                id: tableId,
                monster: monster,
                rollCount: rollCount,
                rows: rows);

            var newRow = new DropTableRow(Guid.NewGuid(), Entities.CreateItem(), new DropRateDenominator(4));
            var addResult = dropTable.AddRow(newRow);

            Assert.False(addResult.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, addResult.Status);

            Assert.Equal(tableId, dropTable.Id);
            AssertExtensions.DeepEqual(monster, dropTable.Monster);
            Assert.Equal(rollCount, dropTable.RollCount);
            AssertExtensions.DeepEqual(rows, dropTable.Rows);
        }
    }
}