using NUnit.Framework;

namespace AoC2023.Day15Part2;

public class Day15Part2
{
    private int Run(IEnumerable<string> data)
    {
        var boxes = new Dictionary<int, List<(string label, int focalLength)>>();
        foreach (var s in data.First().Split(','))
        {
            var (hash, label) = GetHash(s);
            if (s.Contains('='))
            {
                var parts = s.Split("=");
                var focalLength = int.Parse(parts.Last());
                var lens = (label, focalLength);

                boxes.TryAdd(hash, new List<(string label, int focalLength)>());
                var box = boxes[hash];
                if (box.Any(b => b.label == label))
                {
                    boxes.Remove(hash);
                    boxes[hash] = box.Select(l => l.label == label ? lens : l).ToList();
                }
                else
                {
                    box.Add(lens);
                }
            }
            else
            {
                if (boxes.TryGetValue(hash, out var box))
                {
                    boxes[hash] = box.Where(l => l.label != label).ToList();
                }
            }
        }

        return boxes
            .SelectMany(box => box.Value.Select((b, i) => (boxNumber: box.Key, slotNumber: i + 1, b.focalLength)))
            .Sum(v => (1 + v.boxNumber) * v.slotNumber * v.focalLength);
    }

    private (int hash, string label) GetHash(string line)
    {
        var hash = 0;
        var label = "";
        foreach (var t in line)
        {
            if (t == '=' || t == '-')
            {
                return (hash, label);
            }

            label += t;
            hash += t;
            hash *= 17;
            hash %= 256;
        }

        throw new Exception();
    }
      
    private class Day15Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day15Part2/testdata.txt");
            var sut = new Day15Part2();
            Assert.AreEqual(145, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day15Part2/data.txt");
            var sut = new Day15Part2();
            Assert.AreEqual(259333, sut.Run(data));
        }
    }
}