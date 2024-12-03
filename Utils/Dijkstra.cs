namespace Utils;

public class Dijkstra<T> where T : notnull
{
    public Func<T, List<T>> neighbors;
    public Func<T, bool> target;
    public Func<T, bool> valid;
    public Func<T, int> weight;

    public int Compute(T start)
    {
        var dist = new Dictionary<T, int>
        {
            [start] = 0
        };
        var seen = new HashSet<T>();
        var queue = new PriorityQueue<T, int>();
        queue.Enqueue(start, 0);
        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            if (seen.Contains(cell)) continue;
            var current = dist[cell];
            if (target(cell)) return current;
            seen.Add(cell);

            foreach (var neighbor in neighbors(cell))
            {
                if (!valid(neighbor)) continue;
                var newScore = current + weight(neighbor);
                if (!seen.Contains(neighbor)) queue.Enqueue(neighbor, newScore);
                var distanceToNeighbor = dist.TryGetValue(neighbor, out var d) ? d : int.MaxValue;
                if (newScore < distanceToNeighbor)
                {
                    dist[neighbor] = newScore;
                }
            }
        }
        return int.MaxValue;
    }
}