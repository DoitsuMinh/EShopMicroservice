namespace Ordering.Domain.Shared;

public static class StringExtensions
{
    public static bool IsNullOrEmptyOrWhiteSpace(this string? value)
    {
        return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
    }
}
