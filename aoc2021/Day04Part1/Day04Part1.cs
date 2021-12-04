using NUnit.Framework;

namespace aoc2021.Day04Part1;

public class Day04Part1
{
    private int Run(IEnumerable<string> data)
    {
        data = data.ToList();
        var numbers = data.First().Split(",").Select(int.Parse);
        var (boards, results) = BuildState(data);
        var (indexOfWinning, number) = GetIndexOfWinning(numbers, boards, results);
        return number * (Sum(boards[indexOfWinning]) - Sum(Multiply(boards[indexOfWinning], results[indexOfWinning])));
    }

    private static (List<List<int>>, List<List<int>>) BuildState(IEnumerable<string> data)
    {
        return data.Skip(2)
            .Aggregate(
                (new List<List<int>> { new() }, new List<List<int>> { new() }),
                (prev, curr) =>
                {
                    var (boards, results) = prev;
                    if (string.IsNullOrEmpty(curr))
                    {
                        boards.Add(new List<int>());
                        results.Add(new List<int>());
                    }
                    else
                    {
                        var boardNumbers = curr.Split(" ").Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();
                        boards.Last().AddRange(boardNumbers);
                        results.Last().AddRange(boardNumbers.Select(_ => 0));
                    }

                    return prev;
                }
            );
    }

    private (int, int) GetIndexOfWinning(IEnumerable<int> numbers, List<List<int>> boards, List<List<int>> results)
    {
        foreach (var number in numbers)
        {
            var bIndex = 0;
            foreach (var indexOfNumber in boards.Select(board => board.IndexOf(number)))
            {
                if (indexOfNumber != -1)
                {
                    results[bIndex][indexOfNumber] = 1;
                }

                bIndex++;
            }

            var rIndex = 0;
            foreach (var result in results)
            {
                if (SuccesfulBoards.Any(s => Sum(Multiply(s, result)) == 5))
                {
                    return (rIndex, number);
                }

                rIndex++;
            }
        }

        return (-1, -1);
    }

    private static List<int> SuccesfulColumn = new() {0, 5, 10, 15, 20};
    private static List<int> SuccesfulRow = new() {0, 1, 2, 3, 4};

    private readonly List<List<int>> SuccesfulBoards = 
        Enumerable.Range(0, 5).Select((_, i) => SuccesfulColumn.Select(n => n + i).ToList())
        .Concat(Enumerable.Range(0, 5).Select((_, i) => SuccesfulRow.Select(n => n + 5 * i).ToList()))
        .Select(filled => Enumerable.Range(0, 25).Select((_, i) => filled.Contains(i) ? 1 : 0).ToList()).ToList();
    
    private List<int> Multiply(IEnumerable<int> a, IEnumerable<int> b) => a.Zip(b, (c, d) => c * d).ToList();
    private int Sum(IEnumerable<int> a) => a.Aggregate((prev, curr) => prev + curr);

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day04Part1/testdata.txt");
            var sut = new Day04Part1();
            Assert.AreEqual(4512, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day04Part1/data.txt");
            var sut = new Day04Part1();
            Assert.AreEqual(64084, sut.Run(data));
        }
    }
}