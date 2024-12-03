using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day13Part2;

public class Day13Part2
{
    private int Run(IEnumerable<string> dataEnum)
    {
        var data = dataEnum.ToList();
        data.Add("");
        return Run2(data);
    }

    private int Run2(List<string> data)
    {
        var result = 0;
        var lastEmptyLine = 0;
        foreach (var (s, i) in data.Select((s, i) => (s, i)))
        {
            if (s == "")
            {
                var image = data.Skip(lastEmptyLine).Take(i - lastEmptyLine).ToList();
                var (count, isHorizontal) = GetResultFromChangedImages(image);
                lastEmptyLine = i + 1;
                result += isHorizontal ? 100 * count : count;
            }
        }

        return result;
    }

    private (int count, bool isHorizontal) GetResultFromChangedImages(List<string> image)
    {
        var oldResult = GetResult(image, (-1, false));
        for (var y = 0; y < image.Count; y++)
        {
            for (var x = 0; x < image[0].Length; x++)
            {
                var newImage = image.Select(c => c).ToList();
                newImage[y] = newImage[y].Remove(x, 1).Insert(x, (newImage[y][x] == '#' ? '.' : '#').ToString());
                var result = GetResult(newImage, oldResult.Value);
                if (result != null)
                {
                    return result.Value;
                }
            }
        }

        throw new Exception();
    }

    private (int count, bool isHorizontal)? GetResult(List<string> image, (int count, bool isHorizontal) oldResult)
    {
        var flippedImage = image.Flip().ToList();

        for (var startY = 0; startY < image.Count - 1; startY++)
        {
            var ret = (startY + 1, true);
            if (IsMirrored(startY, image) && oldResult != ret)
            {
                return ret;
            }
        }
        for (var startX = 0; startX < flippedImage.Count - 1; startX++)
        {
            var ret = (startX + 1, false);
            if (IsMirrored(startX, flippedImage) && oldResult != ret)
            {
                return (startX + 1, false);
            }
        }

        return null;
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

    private class Day13Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day13Part2/testdata.txt");
            var sut = new Day13Part2();
            Assert.That(sut.Run(data), Is.EqualTo(400));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day13Part2/data.txt");
            var sut = new Day13Part2();
            Assert.That(sut.Run(data), Is.EqualTo(35915));
        }
    }
}