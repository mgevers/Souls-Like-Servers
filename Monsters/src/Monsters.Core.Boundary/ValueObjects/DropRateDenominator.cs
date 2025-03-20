using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class DropRateDenominator : SimpleValueObject<int>
    {
        private static readonly string ErrorMessage = $"{nameof(DropRateDenominator)} must be a power of 2";

        public DropRateDenominator(int value) : base(value)
        {
            if (!IsPowerOfTwo(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), ErrorMessage);
            }
        }

        public static Result<DropRateDenominator> Create(int value)
        {
            if (!IsPowerOfTwo(value))
            {
                return Result.Failure<DropRateDenominator>(ErrorMessage);
            }

            return Result.Success(new DropRateDenominator(value));
        }

        private static bool IsPowerOfTwo(int value)
        {
            return value > 0 && (value & (value - 1)) == 0;
        }
    }
}
