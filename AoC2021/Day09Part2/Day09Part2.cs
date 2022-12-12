using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using NUnit.Framework;

namespace AoC2021.Day09Part2;

public class Day09Part2
{
    private int Run(IList<string> data)
    {
        var rowLength = data.First().Length;
        var board = string.Join("", data).ToList();
        var basins = new List<List<int>>();
        for (var i = 0; i < board.Count; i++)
        {
            if (board[i] != '9' && !basins.SelectMany(b => b).Contains(i))
            {
                basins.Add(RecAddToBasin(new List<int>(), i, board, rowLength));
            }          
        }
        return basins.Select(b => b.Count).OrderBy(c => -c).Take(3).Aggregate(1, (agg, b) => agg * b);
    }

    private List<int> RecAddToBasin(List<int> basin, int i, List<char> board, int rowLength)
    {
        if (board == null) throw new ArgumentNullException(nameof(board));
        var current = board[i];
        if (current == '9' || basin.Contains(i))
        {
            return basin;
        }
        basin.Add(i);
        var all = new List<int> {-rowLength, -1, 1, rowLength};
        var leftSide = new List<int> {-rowLength, 1, rowLength};
        var rightSide = new List<int> {-rowLength, -1, rowLength};
        var neighborDeltas = all;
        if (i % rowLength == rowLength - 1) neighborDeltas = rightSide;
        if (i % rowLength == 0) neighborDeltas = leftSide;
        return neighborDeltas
            .Select(delta => i + delta)
            .Where(pos => pos >= 0 && pos < board.Count)
            .SelectMany(pos => RecAddToBasin(basin, pos, board, rowLength))
            .Distinct()
            .ToList();
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part2/testdata.txt");
            var sut = new Day09Part2();
            Assert.AreEqual(1134, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part2/data.txt");
            var sut = new Day09Part2();
            Assert.AreEqual(1123524, sut.Run(data));
        }
    }
}