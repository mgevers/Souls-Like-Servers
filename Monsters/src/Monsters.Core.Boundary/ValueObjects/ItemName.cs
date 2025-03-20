using CSharpFunctionalExtensions;

namespace Monsters.Core.Boundary.ValueObjects
{
    public class ItemName : SimpleValueObject<string>
    {
        private static readonly string ErrorMessage = $"{nameof(ItemName)} cannot be empty or longer than {MaxLength} characters";

        public const int MaxLength = 128;

        public ItemName(string value) : base(value)
        {
            if (IsInvalid(value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), ErrorMessage);
            }
        }

        public static Result<ItemName> Create(string value)
        {
            if (IsInvalid(value))
            {
                return Result.Failure<ItemName>(ErrorMessage);
            }

            return Result.Success(new ItemName(value));
        }

        private static bool IsInvalid(string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
