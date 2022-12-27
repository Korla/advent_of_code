using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day16Part2;

public class Day16Part2
{
    private record Valve((char, char) Id, int FlowRate, List<(char, char)> Tunnels);
    private record AccumulatedFlow(int Flow, List<(char, char)> Path);

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
        foreach (var valve in new[] {valves.Single(v => v.Id == start)}.Concat(flowValves))
        {
            foreach (var flowValve in flowValves)
            {
                mappings.Add((valve.Id, flowValve.Id), Pathfinding.Dijkstra(valve.Id, flowValve.Id, map));
            }
        }

        var accumulatedFlows = new List<AccumulatedFlow>();
        GetFlows(accumulatedFlows, valves.ToDictionary(v => v.Id, v => v.FlowRate), mappings, 26, flowValves.Select(v => v.Id).ToArray(), new AccumulatedFlow(0, new List<(char, char)>()), start);
        var ordered = accumulatedFlows.OrderByDescending(a => a.Flow).ToList();
        var highest = 0;
        foreach (var (flow, path) in ordered)
        {
            foreach (var a in ordered.Where(a => !a.Path.Intersect(path).Any()))
            {
                highest = Math.Max(highest, flow + a.Flow);
                break;
            }
        }
        return highest;
    }

    private static void GetFlows(
        ICollection<AccumulatedFlow> flows,
        IReadOnlyDictionary<(char, char), int> flowRates,
        IReadOnlyDictionary<((char, char), (char, char)), int> mappings,
        int timeRemaining,
        ICollection<(char, char)> unvisitedValves,
        AccumulatedFlow accumulatedFLow,
        (char, char) currentValve)
    {
        flows.Add(accumulatedFLow);
        var leftToVisit = unvisitedValves.Where(n => mappings[(currentValve, n)] < timeRemaining - 1);
        if (!leftToVisit.Any())
        {
            return;
        }

        foreach (var n in leftToVisit)
        {
            GetFlows(
                flows,
                flowRates,
                mappings,
                timeRemaining - mappings[(currentValve, n)] - 1,
                unvisitedValves.Where(v => v != n).ToList(),
                accumulatedFLow with
                {
                    Flow = accumulatedFLow.Flow + flowRates[n] * (timeRemaining - mappings[(currentValve, n)] - 1),
                    Path = accumulatedFLow.Path.Concat(new[] { n }).ToList()
                },
                n
            );
        }
    }

    private class Day16Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day16Part2/testdata.txt");
            var sut = new Day16Part2();
            Assert.AreEqual(1707, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day16Part2/data.txt");
            var sut = new Day16Part2();
            Assert.AreEqual(2052, sut.Run(data));
        }
    }
}