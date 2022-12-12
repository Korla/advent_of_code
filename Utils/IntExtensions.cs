namespace Utils;

public static class IntExtensions
{
    public static bool IsBetweenInclusive(this int source, int min, int max) => source >= min && source <= max;
}