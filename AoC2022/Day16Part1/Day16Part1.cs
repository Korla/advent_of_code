using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day16Part1;

public class Day16Part1
{
    private record Valve((char, char) Id, int FlowRate, List<(char, char)> Tunnels);

    private int Run(IEnumerable<string> data)
    {
        var pattern = @"Valve ([A-Z][A-Z]) has flow rate=(\d+); tunnels? leads? to valves? (,? ?([A-Z][A-Z]))*";
        var valves = data
            .Select(r => Regex.Match(r, pattern).Groups)
            .Select(match =>
                new Valve(
                    (match[1].Value[0], match[1].Value[1]),
                    int.Parse(match[2].Value),
                    match[4].Captures.Select(c => (c.Value[0], c.Value[1])).ToList()
                )
            )
            .ToList();

        var map = valves.ToDictionary(
            valve => valve.Id,
            valve => (valve.Tunnels, 1)
        );
        var flowValves = valves.Where(v => v.FlowRate > 0).ToList();
        var mappings = new Dictionary<((char, char), (char, char)), int>();
        var start = ('A', 'A');
        foreach (var valve in new[] { valves.Single(v => v.Id == start) }.Concat(flowValves))
        {
            foreach (var flowValve in flowValves)
            {
                mappings.Add((valve.Id, flowValve.Id), Pathfinding.Dijkstra(valve.Id, flowValve.Id, map));
            }
        }

        return GetMaxFlow(valves.ToDictionary(v => v.Id, v => v.FlowRate), mappings, 30, flowValves.Select(v => v.Id).ToArray(), 0, start);
    }

    private static int GetMaxFlow(Dictionary<(char, char), int> flowRates, IReadOnlyDictionary<((char, char), (char, char)), int> mappings, int timeRemaining,
        IEnumerable<(char, char)> unvisitedValves, int accumulatedFLow, (char, char) currentValve)
    {
        var leftToVisit = unvisitedValves.Where(n => mappings[(currentValve, n)] < timeRemaining - 1);
        if (!leftToVisit.Any())
        {
            return accumulatedFLow;
        }

        return leftToVisit.Max(n => GetMaxFlow(
            flowRates,
            mappings,
            timeRemaining - mappings[(currentValve, n)] - 1,
            unvisitedValves.Where(v => v != n),
            accumulatedFLow + flowRates[n] * (timeRemaining - mappings[(currentValve, n)] - 1),
            n
        ));
    }

    private class Day16Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day16Part1/testdata.txt");
            var sut = new Day16Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1651));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day16Part1/data.txt");
            var sut = new Day16Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1584));
        }
    }
}