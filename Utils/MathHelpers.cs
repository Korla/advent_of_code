namespace Utils;

public static class MathHelpers
{
    // LCM from here https://stackoverflow.com/questions/147515/least-common-multiple-for-3-or-more-numbers/29717490#29717490
    public static double LowestCommonMultiple(double a, double b)
    {
        return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
    }

    public static double GreatestCommonDivisor(double a, double b)
    {
        while (true)
        {
            if (b == 0) return a;
            var a1 = a;
            a = b;
            b = a1 % b;
        }
    }
}