using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2021.Day15Part2;

public class Day15Part2
{
    private int Run(IList<string> data)
    {
        var maxY = data.Count;
        var maxX = data[0].Length;
        var map = new Dictionary<(int x, int y), int>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var yIter = 0; yIter < 5; yIter++)
            {
                for (var x = 0; x < data[y].Length; x++)
                {
                    for (var xIter = 0; xIter < 5; xIter++)
                    {
                        var currX = x + xIter * maxX;
                        var currY = y + yIter * maxY;
                        var dist = (data[y][x] - '0' - 1 + xIter + yIter) % 9 + 1;
                        map.Add((currX, currY), dist);
                    }
                }
            }
        }

        // foreach (var keyValuePairs in map.GroupBy(g => g.Key.y).OrderBy(g => g.Key))
        // {
        //     foreach (var keyValuePair in keyValuePairs.OrderBy(g => g.Key.x))
        //     {
        //         Console.Write(keyValuePair.Value.dist);
        //     }
        //     Console.WriteLine("");
        // }

        // return 0;
        var currentNode = (0, 0);
        var targetNode = (maxX * 5 - 1, maxY * 5 - 1);
        return Dijkstra(currentNode, targetNode, map);
    }

    private int Dijkstra((int x, int y) start, (int x, int y) targetNode, Dictionary<(int x, int y), int> map)
    {
        //Mark all nodes unvisited. Create a set of all the unvisited nodes called the unvisited set.
        var unvisited = map.Select(x => x.Key).ToHashSet();
        //Assign to every node a tentative distance value: set it to zero for our initial node and to infinity for all other nodes. The tentative distance of a node v is the length of the shortest path discovered so far between the node v and the starting node. Since initially no path is known to any other vertex than the source itself (which is a path of length zero), all other tentative distances are initially set to infinity. Set the initial node as current.[15]
        var distanceToInitial = map.ToDictionary(x => x.Key, _ => int.MaxValue);
        distanceToInitial[start] = 0;
        var priorityQueue = new PriorityQueue<(int, int), int>();
        priorityQueue.Enqueue(start, 0);
        while (priorityQueue.TryDequeue(out var currentNode, out _))
        {
            //For the current node, consider all of its unvisited neighbors and calculate their tentative distances through the current node.
            //Compare the newly calculated tentative distance to the current assigned value and assign the smaller one.
            //For example, if the current node A is marked with a distance of 6, and the edge connecting it with a neighbor B has length 2, then the distance to B through A will be 6 + 2 = 8.
            //If B was previously marked with a distance greater than 8 then change it to 8. Otherwise, the current value will be kept.
            var unvisitedNeighbors = GetNeighbors(currentNode).Where(n => map.ContainsKey(n) && unvisited.Contains(n));
            foreach (var node in unvisitedNeighbors)
            {
                var dist = map[currentNode] + distanceToInitial[currentNode];
                if(dist < distanceToInitial[node])
                {
                    distanceToInitial[node] = dist;
                    priorityQueue.Enqueue(node, dist);
                }
            }
            
            //When we are done considering all of the unvisited neighbors of the current node, mark the current node as visited and remove it from the unvisited set. A visited node will never be checked again.
            unvisited.Remove(currentNode);

            //If the destination node has been marked visited (when planning a route between two specific nodes) or if the smallest tentative distance among the nodes in the unvisited set is infinity
            //(when planning a complete traversal; occurs when there is no connection between the initial node and remaining unvisited nodes), then stop. The algorithm has finished.
            if (currentNode == targetNode)
            {
                return distanceToInitial[targetNode] - map[(0,0)] + map[targetNode];
            }
            
            //Otherwise, select the unvisited node that is marked with the smallest tentative distance, set it as the new current node, and go back to step 3.
        }

        return int.MaxValue;
    }

    private List<(int, int)> GetNeighbors((int x, int y) i)
    {
        var (currX, currY) = i;
        return new[] {(currX - 1, currY), (currX + 1, currY), (currX, currY - 1), (currX, currY + 1)}.ToList();
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part2/testdata.txt");
            var sut = new Day15Part2();
            Assert.AreEqual(315, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part2/data.txt");
            var sut = new Day15Part2();
            Assert.AreEqual(2874, sut.Run(data));
        }
    }
}