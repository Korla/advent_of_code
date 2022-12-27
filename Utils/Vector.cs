namespace Utils;

public record Vector(int X, int Y);

public static class VectorExtensions
{
    public static Vector From(string input)
    {
        var parts = input.Split(",").ToArray();
        return new Vector(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static Vector Add(this Vector vector, Vector other) => new(vector.X + other.X, vector.Y + other.Y);
    public static Vector Subtract(this Vector vector, Vector other) => new(vector.X - other.X, vector.Y - other.Y);
    public static double Length(this Vector vector) => Math.Sqrt(Math.Pow(Math.Abs(vector.X),2) + Math.Pow(Math.Abs(vector.Y),2));
    public static int ManhattanDistance(this Vector vector, Vector other) => Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);

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
        foreach (var y in yRange.Reverse())
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