namespace FleetMan.Domain.Entities;

using ErrorOr;
using FleetMan.Domain.Errors;

public sealed class ImoNumber
{
    public string Value { get; }

    private ImoNumber(string value)
    {
        Value = value;
    }

    public static ErrorOr<ImoNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Errors.ImoNumber.InvalidFormat;
        }

        if (value.Length != 7)
        {
            return Errors.ImoNumber.InvalidFormat;
        }

        if (!value.All(char.IsDigit))
        {
            return Errors.ImoNumber.InvalidFormat;
        }

        if (!HasValidChecksum(value))
        {
            return Errors.ImoNumber.InvalidChecksum;
        }

        return new ImoNumber(value);
    }

    internal static bool HasValidChecksum(string imo)
    {
        // https://en.wikipedia.org/wiki/IMO_number
        // The checksum of an IMO ship identification number is calculated by multiplying each of the first six digits by a factor of 7 to 2 corresponding to their position from right to left. The rightmost digit of this sum is the check digit.
        // Example for IMO 9074729:
        // (9×7) + (0×6) + (7×5) + (4×4) + (7×3) + (2×2) = 139.
        // The check digit is the rightmost digit of the sum, which is 9 in this case.

        int sum = 0;
        for (int i = 0; i < 6; i++)
        {
            // Convert char to int
            int digit = imo[i] - '0';
            // Leftmost digit is multiplied by 7, next by 6, and so on
            // until the rightmost digit, which is multiplied by 2
            int factor = 7 - i;
            sum += digit * factor;
        }

        int actualCheckDigit = imo[6] - '0';
        int expectedCheckDigit = sum % 10;
        return actualCheckDigit == expectedCheckDigit;
    }

    public override string ToString() => Value;
}
