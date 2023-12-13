using NUnit.Framework;

namespace AoC2023.Day12Part1;

public class Day12Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data.Sum(RunLine);
    }

    private int RunLine(string line)
    {
        var s = line.Split(" ");
        var row = s.First();
        var springs = s.Last().Split(",").Select(int.Parse);
        var count = RecCountPermutations2(row, springs.ToList());
        return count;
    }

    private int RecCountPermutations2(string row, IEnumerable<int> springs)
    {
        if (row.Length == 0)
        {
            return !springs.Any() ? 1 : 0;
        }

        if (!springs.Any())
        {
            return row.Any(t => t == '#') ? 0 : 1;
        }

        if (row.Length < springs.Sum() + springs.Count() - 1) {
            return 0;
        }

        if (row[0] == '.') {
            return RecCountPermutations2(row[1..], springs);
        }
        
        if (row[0] == '#')
        {
            var run = springs.First();
            for (var i = 0; i < run; i++) {
                if (row[i] == '.') {
                    return 0;
                }
            }

            if (row.Length == run)
            {
                return RecCountPermutations2("", springs.Skip(1));
            }

            if (row[run] == '#') {
                return 0;
            }

            return RecCountPermutations2(row[(run + 1)..], springs.Skip(1));
        }
        return RecCountPermutations2("#" + row[1..], springs) +
               RecCountPermutations2("." + row[1..], springs);
    }

    private class Day12Part1Tests
    {
        [TestCase("### 3", 1)]
        [TestCase("? 1", 1)]
        [TestCase("?? 1", 2)]
        [TestCase("??. 1", 2)]
        [TestCase("?.# 1,1", 1)]
        [TestCase("??.# 1,1", 2)]
        [TestCase("???.# 1,1,1", 1)]
        [TestCase("#.# 1,1", 1)]
        [TestCase("???.### 1,1,3", 1)]
        [TestCase("?.## 1,2", 1)]
        [TestCase("## 1,2", 0)]
        [TestCase("??.?? 1,1", 4)]
        [TestCase(".??..??...?##. 1,1,3", 4)]
        [TestCase("?#?#?#?#?#?#?#? 1,3,1,6", 1)]
        [TestCase("????.#...#... 4,1,1", 1)]
        [TestCase("????.######..#####. 1,6,5", 4)]
        [TestCase("?###???????? 3,2,1", 10)]
        [TestCase("?###? 3", 1)]
        [TestCase("?###? 3,1", 0)]
        [TestCase("?###?? 3,2", 0)]
        [TestCase("?###??? 3,2", 1)]
        public void TestData(string line, int expected)
        {
            var sut = new Day12Part1();
            Assert.AreEqual(expected, sut.RunLine(line));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day12Part1/data.txt");
            var sut = new Day12Part1();
            Assert.AreEqual(7090, sut.Run(data));
        }
    }
}