using NUnit.Framework;

namespace aoc2021.Day09Part1;

public class Day09Part1
{
    private int Run(IList<string> data)
    {
        var rowLength = data.First().Length;
        var all = new List<int> {-rowLength, -1, 1, rowLength};
        var leftSide = new List<int> {-rowLength, 1, rowLength};
        var rightSide = new List<int> {-rowLength, -1, rowLength};
        var board = string.Join("", data);
        var total = 0;
        for (var i = 0; i < board.Length; i++)
        {
            var current = board[i];
            var neighbors = all;
            if (i % rowLength == rowLength - 1) neighbors = rightSide;
            if (i % rowLength == 0) neighbors = leftSide;
            var isMinimum = neighbors
                .Select(delta => i + delta)
                .Where(pos => pos >= 0 && pos < board.Length)
                .Select(pos => board[pos])
                .All(neighbor => current < neighbor);
            total += isMinimum ? board[i] - '0' + 1 : 0;
            
            // if not 9
            // is neighbor in basin, add to that basin
            // if not create new basin
        }
        return total;
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day09Part1/testdata.txt");
            var sut = new Day09Part1();
            Assert.AreEqual(15, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day09Part1/data.txt");
            var sut = new Day09Part1();
            Assert.AreEqual(526, sut.Run(data));
        }
    }
}