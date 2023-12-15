using NUnit.Framework;

namespace AoC2023.Day15Part1;

public class Day15Part1
{
    private int Run(IEnumerable<string> data)
    {
        var line = data.First();
        var sum = 0;
        var currentValue = 0;
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == ',')
            {
                sum += currentValue;
                currentValue = 0;
            }
            else
            {
                currentValue += line[i];
                currentValue *= 17;
                currentValue %= 256;
            }
        }
        return sum + currentValue;
    }
      
    private class Day15Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part1/testdata.txt");
            var sut = new Day15Part1();
            Assert.AreEqual(1320, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part1/data.txt");
            var sut = new Day15Part1();
            Assert.AreEqual(509167, sut.Run(data));
        }
    }
}