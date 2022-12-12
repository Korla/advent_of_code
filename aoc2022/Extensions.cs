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

public static class IntExtensions
{
    public static bool IsBetweenInclusive(this int source, int min, int max) => source >= min && source <= max;
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

public static class Pathfinding
{
    public static int Dijkstra((int x, int y) start, (int x, int y) targetNode, IReadOnlyDictionary<(int x, int y), (List<(int x, int y)> neighbors, int dist)> map)
    {
        //Mark all nodes unvisited. Create a set of all the unvisited nodes called the unvisited set.
        var unvisited = map.Select(x => x.Key).ToHashSet();
        //Assign to every node a tentative distance value: set it to zero for our initial node and to infinity for all other nodes. The tentative distance of a node v is the length of the shortest path discovered so far between the node v and the starting node. Since initially no path is known to any other vertex than the source itself (which is a path of length zero), all other tentative distances are initially set to infinity. Set the initial node as current.[15]
        var distanceToInitial = map.ToDictionary(x => x.Key, _ => int.MaxValue);
        distanceToInitial[start] = 0;
        var currentNode = start;
        while (unvisited.Any())
        {
            //For the current node, consider all of its unvisited neighbors and calculate their tentative distances through the current node.
            //Compare the newly calculated tentative distance to the current assigned value and assign the smaller one.
            //For example, if the current node A is marked with a distance of 6, and the edge connecting it with a neighbor B has length 2, then the distance to B through A will be 6 + 2 = 8.
            //If B was previously marked with a distance greater than 8 then change it to 8. Otherwise, the current value will be kept.
            var unvisitedNeighbors = map[currentNode].neighbors.Where(n => map.ContainsKey(n) && unvisited.Contains(n));
            foreach (var node in unvisitedNeighbors)
            {
                var totalDist = map[currentNode].dist + distanceToInitial[currentNode];
                distanceToInitial[node] = Math.Min(totalDist, distanceToInitial[node]);
            }
            
            //When we are done considering all of the unvisited neighbors of the current node, mark the current node as visited and remove it from the unvisited set. A visited node will never be checked again.
            unvisited.Remove(currentNode);

            //If the destination node has been marked visited (when planning a route between two specific nodes) or if the smallest tentative distance among the nodes in the unvisited set is infinity
            //(when planning a complete traversal; occurs when there is no connection between the initial node and remaining unvisited nodes), then stop. The algorithm has finished.
            if (currentNode == targetNode)
            {
                return distanceToInitial[targetNode] - map[(0,0)].dist + map[targetNode].dist;
            }
            
            //Otherwise, select the unvisited node that is marked with the smallest tentative distance, set it as the new current node, and go back to step 3.
            var nextNode = unvisited.ToDictionary(x => x, x => distanceToInitial[x]).MinBy(x => x.Value);
            currentNode = nextNode.Key;
        }

        return int.MaxValue;
    }
}