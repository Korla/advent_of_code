using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day14Part2;

public class Day14Part2
{
    private int Run(IEnumerable<string> data)
    {
        var source = new Vector(500, 0);
        var map = new HashSet<Vector>();
        foreach (var row in data)
        {
            foreach (var vector in row.Split(" -> ").Select(VectorExtensions.From).Pairwise((v1, v2) =>
                     {
                         var xIsSame = v1.X == v2.X;
                         if (xIsSame)
                         {
                             var x = v1.X;
                             var start = Math.Min(v1.Y, v2.Y);
                             var end = Math.Max(v1.Y, v2.Y);
                             var count = end - start + 1;
                             var enumerable = Enumerable.Range(start, count).Select(y => new Vector(x, y));
                             return enumerable;
                         }
                         else
                         {
                             var y = v1.Y;
                             var start = Math.Min(v1.X, v2.X);
                             var end = Math.Max(v1.X, v2.X);
                             var count = end - start + 1;
                             var enumerable = Enumerable.Range(start, count).Select(x => new Vector(x, y));
                             return enumerable;
                         }
                     }).SelectMany(v => v))
            {
                map.Add(vector);
            }
        }

        var maxY = map.Select(v => v.Y).Max() + 2;

        var count = 0;
        while (true)
        {
            var fallingSand = source with { };
            var isAtRest = false;

            while (!isAtRest)
            {
                var fallingSandDown = fallingSand with { Y = fallingSand.Y + 1 };
                if (!map.Contains(fallingSandDown) && fallingSand.Y < maxY - 1)
                {
                    fallingSand = fallingSandDown;
                    continue;
                }
                var fallingSandDownLeft = new Vector(fallingSand.X - 1, fallingSand.Y + 1);
                if (!map.Contains(fallingSandDownLeft) && fallingSand.Y < maxY - 1)
                {
                    fallingSand = fallingSandDownLeft;
                    continue;
                }
                var fallingSandDownRight = new Vector(fallingSand.X + 1, fallingSand.Y + 1);
                if (!map.Contains(fallingSandDownRight) && fallingSand.Y < maxY - 1)
                {
                    fallingSand = fallingSandDownRight;
                    continue;
                }

                isAtRest = true;
                map.Add(fallingSand);
                count++;

                if (fallingSand.Equals(source))
                {
                    return count;
                }
            }
        }
    }

    private class Day14Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day14Part2/testdata.txt");
            var sut = new Day14Part2();
            Assert.That(sut.Run(data), Is.EqualTo(93));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day14Part2/data.txt");
            var sut = new Day14Part2();
            Assert.That(sut.Run(data), Is.EqualTo(23925));
        }
    }
}