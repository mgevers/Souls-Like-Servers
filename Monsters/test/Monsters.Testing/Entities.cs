using Common.Core.Boundary;
using Monsters.Core.Boundary.ValueObjects;
using Monsters.Core.Domain;

namespace Monsters.Testing
{
    public static class Entities
    {
        public static Monster CreateMonster(
            Guid? id = null,
            MonsterName? name = null,
            MonsterLevel? level = null,
            SoulsAttributeSet? attributeSet = null)
        {
            return new Monster(
                id: id ?? Guid.NewGuid(),
                name: name ?? new MonsterName("Skeleton"),
                level: level ?? new MonsterLevel(1),
                attributeSet: attributeSet ?? new SoulsAttributeSet());
        }

        public static Item CreateItem(
            Guid? id = null,
            ItemName? name = null,
            SoulsAttributeSet? attributeSet = null)
        {
            return new Item(
                id: id ?? Guid.NewGuid(),
                name: name ?? new ItemName("Sword"),
                attributeSet: attributeSet ?? new SoulsAttributeSet());
        }

        public static DropTable CreateEmptyDropTable(
            Guid? tableId = null,
            Monster? monster = null,
            RollCount? rollCount = null)
        {

            return new DropTable(
                id: tableId ?? Guid.NewGuid(),
                monster: monster ?? CreateMonster(),
                rollCount: rollCount ?? new RollCount(1),
                rows: []);
        }

        public static DropTable CreateDropTable(
            IReadOnlyCollection<DropTableRow> rows,
            Guid? tableId = null,
            Monster? monster = null,
            RollCount? rollCount = null)
        {

            return new DropTable(
                id: tableId ?? Guid.NewGuid(),
                monster: monster ?? CreateMonster(),
                rollCount: rollCount ?? new RollCount(1),
                rows: rows);
        }

        public static DropTableRow CreateDropTableRow(
            Guid? rowId = null,
            Item? item = null,
            DropRateDenominator? dropRate = null)
        {
            return new DropTableRow(
                id: rowId ?? Guid.NewGuid(),
                item: item ?? CreateItem(),
                dropRateDenominator: dropRate ?? new DropRateDenominator(2));
        }
    }
}
