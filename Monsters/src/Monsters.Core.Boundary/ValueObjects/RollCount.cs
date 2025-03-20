using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class RollCount : SimpleValueObject<int>
    {
        private static readonly string ErrorMessage = $"{nameof(RollCount)} must be greater than or equal to {Min} and less than or equal to {Max}";

        public const int Min = 1;
        public const int Max = int.MaxValue;

        public RollCount(int value) : base(value)
        {
            if (IsInvalid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), ErrorMessage);
            }
        }

        public static Result<RollCount> Create(int value)
        {
            if (IsInvalid(value))
            {
                return Result.Failure<RollCount>(ErrorMessage);
            }

            return Result.Success(new RollCount(value));
        }

        private static bool IsInvalid(int value)
        {
            return value < Min || value > Max;
        }
    }
}
