using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class MonsterLevel : SimpleValueObject<int>
    {
        private static readonly string ErrorMessage = $"{nameof(MonsterLevel)} cannot be less than {Min} or greater than {Max}";

        public const int Min = 1;
        public const int Max = 100;

        public MonsterLevel(int value) : base(value)
        {
            if (IsInvalid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), ErrorMessage);
            }
        }

        public static Result<MonsterLevel> Create(int value)
        {
            if (IsInvalid(value))
            {
                return Result.Failure<MonsterLevel>(ErrorMessage);
            }

            return Result.Success(new MonsterLevel(value));
        }

        private static bool IsInvalid(int value)
        {
            return value < Min || value > Max;
        }
    }
}
