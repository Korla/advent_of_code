using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day12Part2;

public class Day12Part2
{
    private double Run(IEnumerable<string> data)
    {
        return data.Sum(RunLine);
    }

    private double RunLine(string line)
    {
        var s = line.Split(" ");
        var row = s.First();
        var springs = s.Last().Split(",").Select(int.Parse).ToList();
        var count = MemoizedRecCountPermutations((
            string.Join("?", Enumerable.Range(0, 5).Select(_ => row)),
            springs.Concat(springs).Concat(springs).Concat(springs).Concat(springs)
        ));
        return count;
    }

    private static readonly Func<(string row, IEnumerable<int> springs), double> MemoizedRecCountPermutations = Memoize(RecCountPermutations);

    private static double RecCountPermutations((string row, IEnumerable<int> springs) args)
    {
        var (row, springs) = args;
        if (row.Length == 0)
        {
            return !springs.Any() ? 1 : 0;
        }

        if (!springs.Any())
        {
            return row.Any(t => t == '#') ? 0 : 1;
        }

        if (row.Length < springs.Sum() + springs.Count() - 1)
        {
            return 0;
        }

        if (row[0] == '.')
        {
            return MemoizedRecCountPermutations((row[1..], springs));
        }

        if (row[0] == '#')
        {
            var run = springs.First();
            for (var i = 0; i < run; i++)
            {
                if (row[i] == '.')
                {
                    return 0;
                }
            }

            if (row.Length == run)
            {
                return MemoizedRecCountPermutations(("", springs.Skip(1)));
            }

            if (row[run] == '#')
            {
                return 0;
            }

            return MemoizedRecCountPermutations((row[(run + 1)..], springs.Skip(1)));
        }
        return MemoizedRecCountPermutations(("#" + row[1..], springs)) +
               MemoizedRecCountPermutations(("." + row[1..], springs));
    }

    private static Func<(string row, IEnumerable<int> springs), double> Memoize(Func<(string row, IEnumerable<int> springs), double> func)
    {
        var cache = new Dictionary<string, double>();
        return a =>
        {
            var (row, springs) = a;
            var key = row + "|" + string.Join("", springs);
            if (cache.TryGetValue(key, out var value))
                return value;
            value = func(a);
            cache.Add(key, value);
            return value;
        };
    }

    private class Day12Part2Tests
    {
        [TestCase("???.### 1,1,3", 1)]
        [TestCase(".??..??...?##. 1,1,3", 16384)]
        [TestCase("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
        [TestCase("????.#...#... 4,1,1", 16)]
        [TestCase("????.######..#####. 1,6,5", 2500)]
        [TestCase("?###???????? 3,2,1", 506250)]
        public void TestData(string line, double expected)
        {
            var sut = new Day12Part2();
            Assert.That(sut.RunLine(line), Is.EqualTo(expected));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part2/data.txt");
            var sut = new Day12Part2();
            Assert.That(sut.Run(data), Is.EqualTo(6792010726878));
        }
    }
}