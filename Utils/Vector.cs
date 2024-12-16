namespace Utils;

public record Vector(int X, int Y)
{
    public static readonly Vector Origo = new(0, 0);
    public static readonly Vector Up = new(0, -1);
    public static readonly Vector Right = new(1, 0);
    public static readonly Vector Down = new(0, 1);
    public static readonly Vector Left = new(-1, 0);
    public static readonly Vector UpRight = Up.Add(Right);
    public static readonly Vector UpLeft = Up.Add(Left);
    public static readonly Vector DownRight = Down.Add(Right);
    public static readonly Vector DownLeft = Down.Add(Left);

    public static readonly List<Vector> CardinalDirections = new()
    {
        Up,
        Right,
        Down,
        Left
    };

    public static readonly List<Vector> AllNeighbors = new()
    {
        Up,
        UpRight,
        Right,
        DownRight,
        Down,
        DownLeft,
        Left,
        UpLeft
    };
    
    public static Vector operator +(Vector a, Vector b) => a.Add(b);
    public static Vector operator -(Vector a, Vector b) => a.Subtract(b);
    public static Vector operator %(Vector a, Vector b) => a.Modulo(b);
    public static Vector operator *(Vector a, int scale) => a.Multiply(scale);
    public static Vector operator /(Vector a, int scale) => a.Divide(scale);

    public static Vector Parse(string s)
    {
        return s switch
        {
            "U" or "^" => Up,
            "R" or ">" => Right,
            "D" or "v" => Down,
            "L" or "<" => Left,
            _ => throw new Exception()
        };
    }

    public static Vector Parse(char c) => Parse(c.ToString());

    public override string ToString()
    {
        return $"({X},{Y})";
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
    public static Vector Subtract(this Vector vector, Vector other) => new(vector.X - other.X, vector.Y - other.Y);
    public static Vector Multiply(this Vector vector, int scale) => new(vector.X * scale, vector.Y * scale);
    public static Vector Divide(this Vector vector, int scale) => new(vector.X / scale, vector.Y / scale);
    public static Vector Modulo(this Vector vector, Vector other) => new(vector.X % other.X, vector.Y % other.Y);

    public static Vector Rotate(this Vector vector)
    {
        if (vector == Vector.Up) return Vector.Right;
        if (vector == Vector.Right) return Vector.Down;
        if (vector == Vector.Down) return Vector.Left;
        if (vector == Vector.Left) return Vector.Up;
        throw new Exception("Invalid vector");
    }

    public static Vector RotateACW(this Vector vector)
    {
        if (vector == Vector.Up) return Vector.Left;
        if (vector == Vector.Right) return Vector.Up;
        if (vector == Vector.Down) return Vector.Right;
        if (vector == Vector.Left) return Vector.Down;
        throw new Exception("Invalid vector");
    }

    public static LongVector Add(this LongVector vector, LongVector other) =>
        vector with { X = vector.X + other.X, Y = vector.Y + other.Y };
    public static LongVector Multiply(this LongVector vector, int scale) => new(vector.X * scale, vector.Y * scale);

    public static double Length(this Vector vector) => Math.Sqrt(Math.Pow(Math.Abs(vector.X), 2) + Math.Pow(Math.Abs(vector.Y), 2));
    public static int ManhattanDistance(this Vector vector, Vector other) => Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);
    public static long ManhattanDistance(this LongVector vector, LongVector other) => Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);

    public static void Print(this IEnumerable<Vector> vectors, string value = "X") {
        Print(vectors.Select(v => (v, value)));
    }

    public static void Print(this IEnumerable<(Vector vector, string? value)> vectors)
    {
        vectors = vectors.ToList();

        if (!vectors.Any())
        {
            Console.WriteLine("No elements");
            return;
        }

        var xMin = vectors.Select(v => v.vector.X).Min();
        var xMax = vectors.Select(v => v.vector.X).Max();
        var yMin = vectors.Select(v => v.vector.Y).Min();
        var yMax = vectors.Select(v => v.vector.Y).Max();
        var xRange = Enumerable.Range(xMin, xMax - xMin + 1).ToArray();
        var yRange = Enumerable.Range(yMin, yMax - yMin + 1).ToArray();
        var xNumberLength = Math.Max(Math.Abs(xMax), Math.Abs(xMin)).ToString().Length;
        var yNumberMaxLength = Math.Max(Math.Abs(yMax), Math.Abs(yMin)).ToString().Length;
        foreach (var i in Enumerable.Range(0, xNumberLength))
        {
            foreach (var i1 in Enumerable.Range(0, yNumberMaxLength))
            {
                Console.Write(" ");
            }
            Console.Write(" ");

            foreach (var x in xRange)
            {
                var s = x.ToString();
                if (i < s.Length)
                {
                    Console.Write(s[i]);
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }
        foreach (var y in yRange)
        {
            Console.Write(y.ToString());
            var yNumberLength = y.ToString().Length;
            foreach (var i in Enumerable.Range(0, yNumberMaxLength - yNumberLength + 1))
            {
                Console.Write(" ");
            }
            foreach (var x in xRange)
            {
                var vector = vectors.FirstOrDefault(v => v.vector == new Vector(x, y));
                Console.Write(vector.value ?? ".");
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