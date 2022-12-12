using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace AoC2021.Day11Part2;

public class Day11Part2
{
    private int Run(IList<string> data)
    {
        var colCount = data.First().Length;
        var allDeltas = new[]{ -colCount - 1, -colCount, -colCount + 1, -1, +1, +colCount - 1, +colCount, +colCount + 1 };
        var leftDeltas = new[]{ -colCount, -colCount + 1, +1, +colCount, +colCount + 1 };
        var rightDeltas = new[]{ -colCount - 1, -colCount, -1, +colCount - 1, +colCount };
        var board = data.SelectMany(s => s.Select(v => v - '0')).ToList();
        var gen = 0;
        while (board.Any(c => c != 0))
        {
            gen++;
            for (var curr = 0; curr < board.Count; curr++)
            {
                board[curr]++;
            }

            var flashed = true;
            while (flashed)
            {
                // Console.WriteLine("Flashing");
                // LogBoard(board, colCount);

                flashed = false;
                for (var curr = 0; curr < board.Count; curr++)
                {
                    if (board[curr] > 9)
                    {
                        board[curr] = 0;
                        flashed = true;
                        var col = curr % colCount;
                        var deltas = col == 0 ? leftDeltas : col == colCount - 1 ? rightDeltas : allDeltas;
                        foreach (var neighbor in deltas.Select(d => curr + d)
                                     .Where(pos => pos >= 0 && pos < board.Count))
                        {
                            board[neighbor] = board[neighbor] == 0 ? 0 : board[neighbor] + 1;
                        }

                        // Console.WriteLine($"Flashed {curr}");
                        // LogBoard(board, colCount);
                    }
                }
            }
            Console.WriteLine($"Generation {gen + 1}");
            LogBoard(board, colCount);
        }

        // Console.WriteLine("Final board");
        // LogBoard(board, colCount);
        return gen;
    }

    private void LogBoard(IReadOnlyCollection<int> board, int colCount)
    {
        for (var i = 0; i < colCount; i++)
        {
            Console.WriteLine($"{string.Join(", ", board.Skip(i * colCount).Take(colCount))}");
        }
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day11Part2/testdata.txt");
            var sut = new Day11Part2();
            Assert.AreEqual(195, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day11Part2/data.txt");
            var sut = new Day11Part2();
            Assert.AreEqual(360, sut.Run(data));
        }
    }
}