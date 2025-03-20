using Common.Core.Boundary;

namespace Presentation.Application.API.Requests
{
    public class AddMonsterRequest
    {
        public Guid MonsterId { get; set; }
        public string MonsterName { get; set; } = string.Empty;
        public int MonsterLevel { get; set; }
        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }

    public class UpdateMonsterRequest
    {
        public Guid MonsterId { get; set; }
        public string MonsterName { get; set; } = string.Empty;
        public int MonsterLevel { get; set; }
        public SoulsAttributeSet AttributeSet { get; set; } = null!;
    }
}
