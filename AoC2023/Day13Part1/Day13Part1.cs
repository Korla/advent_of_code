using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day13Part1;

public class Day13Part1
{
    private int Run(IEnumerable<string> dataEnum)
    {
        var data = dataEnum.ToList();
        data.Add("");
        var result = 0;
        var lastEmptyLine = 0;
        foreach (var (s, i) in data.Select((s, i) => (s, i)))
        {
            if (s == "")
            {
                var (count, isHorizontal) = GetResult(data.Skip(lastEmptyLine).Take(i - lastEmptyLine).ToList());
                lastEmptyLine = i + 1;
                result += isHorizontal ? 100 * count : count;
            }
        }

        return result;
    }

    private (int count, bool isHorizontal) GetResult(List<string> image)
    {
        var flippedImage = image
            .SelectMany(s => s.Select((c, i) => (c, i)))
            .GroupBy(a1 => a1.i, a2 => a2.c)
            .Select(a => string.Join("", a.Select(b => b)))
            .ToList();

        for (var startY = 0; startY < image.Count - 1; startY++)
        {
            if (IsMirrored(startY, image))
            {
                return (startY + 1, true);
            }
        }
        for (var startX = 0; startX < flippedImage.Count - 1; startX++)
        {
            if (IsMirrored(startX, flippedImage))
            {
                return (startX + 1, false);
            }
        }

        throw new Exception();
    }

    private bool IsMirrored(int start, List<string> image)
    {
        var delta = 0;
        while (start - delta >= 0 && start + 1 + delta < image.Count)
        {
            if (image[start - delta] != image[start + 1 + delta])
            {
                return false;
            }
            delta++;
        }

        return true;
    }

    private class Day13Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part1/testdata.txt");
            var sut = new Day13Part1();
            Assert.That(sut.Run(data), Is.EqualTo(405));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part1/data.txt");
            var sut = new Day13Part1();
            Assert.That(sut.Run(data), Is.EqualTo(36041));
        }
    }
}