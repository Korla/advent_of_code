﻿namespace utils;

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