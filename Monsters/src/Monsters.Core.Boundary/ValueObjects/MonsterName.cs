using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class MonsterName : SimpleValueObject<string>
    {
        private static readonly string ErrorMessage = $"{nameof(MonsterName)} cannot be empty or longer than {MaxLength} characters";

        public const int MaxLength = 128;

        public MonsterName(string value) : base(value)
        {
            if (IsInvalid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), ErrorMessage);
            }
        }

        public static Result<MonsterName> Create(string value)
        {
            if (IsInvalid(value))
            {
                return Result.Failure<MonsterName>(ErrorMessage);
            }

            return Result.Success(new MonsterName(value));
        }

        private static bool IsInvalid(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
