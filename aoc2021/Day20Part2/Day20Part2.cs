using NUnit.Framework;

namespace aoc2021.Day20Part2;

public class Day20Part2
{
    private readonly (int x, int y)[] _deltas =
    {
        (-1, -1), (0, -1), (1, -1),
        (-1, 0), (0, 0), (1, 0),
        (-1, 1), (0, 1), (1, 1),
    };

    private int Run(string[] data)
    {
        var algorithm = data.First();
        var boardInput = data.Skip(2).ToList();
        var board = new Dictionary<(int x, int y), char>();
        for (var y = 0; y < boardInput.Count; y++)
        {
            for (var x = 0; x < boardInput[y].Length; x++)
            {
                board.Add((x, y), boardInput[y][x]);
            }
        }

        var gen = 0;
        var res = Enumerable.Range(0, 50).Aggregate(board, (prev, _) => Enhance(prev, algorithm, gen++));
        return res.GroupBy(x => x.Value).First(g => g.Key == '#').Count();
    }

    private void Print(Dictionary<(int x, int y),char> board)
    {
        var size = (int) Math.Sqrt(board.Count);
        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                Console.Write(board[(x,y)]);
            }
            Console.WriteLine();
        }
    }

    private Dictionary<(int x, int y), char> Enhance(Dictionary<(int x, int y), char> board, string algorithm, int gen)
    {
        var def = algorithm[0] == '.' ? '.' : gen % 2 == 0 ? '.' : '#';
        var oldSize = (int) Math.Sqrt(board.Count);
        var newMax = oldSize + 1;
        var extended = new Dictionary<(int x, int y), char>();
        for (var y = 0; y < oldSize; y++)
        {
            var mappedY = y + 1;
            extended.Add((0, mappedY), def);
            extended.Add((newMax, mappedY), def);
            for (var x = 0; x < oldSize; x++)
            {
                var mappedX = x + 1;
                if (y == 0)
                {
                    extended.Add((mappedX, 0), def);
                    extended.Add((mappedX, newMax), def);
                }

                extended.Add((mappedX, mappedY), board[(x, y)]);
            }
        }

        extended.Add((0, 0), def);
        extended.Add((0, newMax), def);
        extended.Add((newMax, 0), def);
        extended.Add((newMax, newMax), def);
        
        var nextBoard = new Dictionary<(int x, int y), char>();
        foreach (var ((x, y), vOld) in extended)
        {
            var a = _deltas.Select(p => (p.x + x, p.y + y)).ToList();
            var b = a.Select(p => extended.TryGetValue(p, out var v) ? v : def).ToList();
            var c = b.Select(v => v == '.' ? "0" : "1").ToList();
            var valueString = string.Join("", c);
            var value = Convert.ToInt32(valueString, 2);
            nextBoard[(x, y)] = algorithm[value];
        }

        return nextBoard;
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day20Part2/testdata.txt");
            var sut = new Day20Part2();
            Assert.AreEqual(3351, sut.Run(data));
        }
        
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day20Part2/data.txt");
            var sut = new Day20Part2();
            Assert.AreEqual(15653, sut.Run(data));
        }
    }
}