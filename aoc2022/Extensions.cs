using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2022;

public record Vector(int X, int Y);

public static class VectorExtensions
{
    public static Vector Add(this Vector vector, Vector other) => new(vector.X + other.X, vector.Y + other.Y);
    public static Vector Subtract(this Vector vector, Vector other) => new(vector.X - other.X, vector.Y - other.Y);
    public static double Length(this Vector vector) => Math.Sqrt(Math.Pow(Math.Abs(vector.X),2) + Math.Pow(Math.Abs(vector.Y),2));

    public static void Print(this IEnumerable<Vector> vectors)
    {
        var xMin = vectors.Select(v => v.X).Min();
        var xMax = vectors.Select(v => v.X).Max();
        var yMin = vectors.Select(v => v.Y).Min();
        var yMax = vectors.Select(v => v.Y).Max();
        var xRange = Enumerable.Range(xMin, xMax-xMin + 1).ToArray();
        var yRange = Enumerable.Range(yMin, yMax-yMin + 1).ToArray();
        foreach (var x in xRange)
        {
            foreach (var y in yRange)
            {
                var vector = new Vector(x, y);
                Console.Write(vectors.Any(v => v == vector) ? "X" : ".");
            }
            Console.WriteLine();
        }
        Console.WriteLine("------------------------------------");
    }
}

public static class EnumerableExtensions
{
    public static int Multiply(this IEnumerable<int> source) => source.Aggregate(1, (prev, curr) => prev * curr);
    public static int Multiply<T>(this IEnumerable<T> source, Func<T, int> func) => source.Aggregate(1, (prev, curr) => prev * func(curr));

    public static IEnumerable<IList<T>> SlidingWindowValues<T>(this IEnumerable<T> source, int windowSize)
    {
        var windows = Enumerable.Range(0, windowSize)
            .Select(_ => new List<T>())
            .ToList();

        var i = 0;
        using var iter = source.GetEnumerator();
        while (iter.MoveNext())
        {
            var c = Math.Min(i + 1, windowSize);
            for (var j = 0; j < c; j++)
            {
                windows[(i - j) % windowSize].Add(iter.Current);
            }
            if (i >= windowSize - 1)
            {
                var previous = (i + 1) % windowSize;
                yield return windows[previous];
                windows[previous] = new List<T>();
            }
            i++;
        }
    }
}