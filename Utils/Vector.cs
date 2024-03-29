﻿using System.Numerics;

namespace Utils;

public record Vector(int X, int Y)
{
    public static Vector Origo = new(0, 0);
    public static Vector Up = new(0, -1);
    public static Vector Right = new(1, 0);
    public static Vector Down = new(0, 1);
    public static Vector Left = new(-1, 0);

    public static List<Vector> CardinalDirections = new()
    {
        Up,
        Right,
        Down,
        Left
    };

    public static Vector Parse(string s)
    {
        switch (s)
        {
            case "U": return Up;
            case "R": return Right;
            case "D": return Down;
            case "L": return Left;
            default: throw new Exception();
        }
    }
}
public record LongVector(long X, long Y);

public static class VectorExtensions
{
    public static Vector From(string input)
    {
        var parts = input.Split(",").ToArray();
        return new Vector(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static Vector Add(this Vector vector, Vector other) => new(vector.X + other.X, vector.Y + other.Y);
    public static Vector Multiply(this Vector vector, int scale) => new(vector.X * scale, vector.Y * scale);

    public static LongVector Add(this LongVector vector, LongVector other) =>
        vector with {X = vector.X + other.X, Y = vector.Y + other.Y};

    public static Vector Subtract(this Vector vector, Vector other) => new(vector.X - other.X, vector.Y - other.Y);
    public static double Length(this Vector vector) => Math.Sqrt(Math.Pow(Math.Abs(vector.X),2) + Math.Pow(Math.Abs(vector.Y),2));
    public static int ManhattanDistance(this Vector vector, Vector other) => Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);
    public static long ManhattanDistance(this LongVector vector, LongVector other) => Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);

    public static void Print(this IEnumerable<Vector> vectors)
    {
        vectors = vectors.ToList();
        var xMin = vectors.Select(v => v.X).Min();
        var xMax = vectors.Select(v => v.X).Max();
        var yMin = vectors.Select(v => v.Y).Min();
        var yMax = vectors.Select(v => v.Y).Max();
        var xRange = Enumerable.Range(xMin, xMax-xMin + 1).ToArray();
        var yRange = Enumerable.Range(yMin, yMax-yMin + 1).ToArray();
        var xNumberLength = Math.Max(Math.Abs(xMax), Math.Abs(xMin)).ToString().Length;
        var yNumberLength = Math.Max(Math.Abs(yMax), Math.Abs(yMin)).ToString().Length;
        foreach (var i in Enumerable.Range(0, xNumberLength))
        {
            foreach (var i1 in Enumerable.Range(0, yNumberLength))
            {
                Console.Write(" ");
            }
            Console.Write(" ");

            foreach (var x in xRange)
            {
                Console.Write(x.ToString()[i]);
            }
            Console.WriteLine();
        }
        foreach (var y in yRange)
        {
            Console.Write(y.ToString());
            Console.Write(" ");
            foreach (var x in xRange)
            {
                var vector = new Vector(x, y);
                Console.Write(vectors.Any(v => v == vector) ? "X" : ".");
            }
            Console.WriteLine();
        }
        Console.WriteLine("------------------------------------");
    }
}

public class LongMatrix
{
    public HashSet<LongVector> rows;
    public long MaxY;

    public LongMatrix(int size)
    {
        rows = Enumerable.Range(0, size).Select(x => new LongVector(x, 0)).ToHashSet();
    }

    public LongMatrix(IEnumerable<LongVector> input)
    {
        rows = input.ToHashSet();
    }

    public bool Contains(LongVector vector)
    {
        return rows.Contains(vector);
    }

    public void AddVector(LongVector vector)
    {
        MaxY = Math.Max(MaxY, vector.Y);
        rows.Add(vector);
    }

    public LongMatrix Add(LongVector nextHorizontalPos)
    {
        return new LongMatrix(rows.Select(v => v.Add(nextHorizontalPos)).ToHashSet());
    }

    public bool Overlaps(IEnumerable<LongVector> elements)
    {
        return rows.Overlaps(elements);
    }
}