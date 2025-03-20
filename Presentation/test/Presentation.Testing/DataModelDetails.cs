using Common.Core.Boundary;
using Presentation.Core.DataModels;

namespace Presentation.Testing
{
    public static class DataModelDetails
    {
        public static ItemDetail CreateItem(
            Guid? id = null,
            SoulsAttributeSet? attributeSet = null,
            string itemName = "Sword")
        {
            return new ItemDetail(
                id: id ?? Guid.NewGuid(),
                itemName: itemName,
                attributeSet: attributeSet ?? new SoulsAttributeSet());
        }

        public static MonsterDetail CreateMonster(
            Guid? id = null,
            SoulsAttributeSet? attributeSet = null,
            string monsterName = "Goblin",
            int monsterLevel = 1)
        {
            return new MonsterDetail(
                id: id ?? Guid.NewGuid(),
                monsterName: monsterName,
                monsterLevel: monsterLevel,
                attributeSet: attributeSet ?? new SoulsAttributeSet());
        }

        public static DropRateDetail CreateDropRateDetail(
            Guid? id = null,
            ItemDetail? item = null,
            double dropRate = .5)
        {
            return new DropRateDetail(
                id: id ?? Guid.NewGuid(),
                item: item ?? CreateItem(),
                dropRate: dropRate);
        }

        public static DropTableDetail CreateDropTable(
            Guid? id = null,
            int rollCount = 1,
            MonsterDetail? monster = null,
            IReadOnlyCollection<DropRateDetail>? rows = null)
        {            
            return new DropTableDetail(
                id: id ?? Guid.NewGuid(),
                monster: monster ?? CreateMonster(),
                rollCount: rollCount,
                rows: rows?.ToList() ?? []);
        }
    }
}
