using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2023.Day06Part1;

public class Day06Part1
{
    // Time:      7  15   30
    // Distance:  9  40  200
    // Time:        60     94     78     82
    // Distance:   475   2138   1015   1650
    private int Run(List<(int time, int distance)> data)
    {
        return data
            .Select(d =>
            {
                var (time, distance) = d;
                var distanceBeaten = false;
                var timePulledBack = -1;
                while (!distanceBeaten)
                {
                    timePulledBack++;
                    var distanceRun = timePulledBack * (time - timePulledBack);
                    distanceBeaten = distanceRun > distance;
                }

                return time + 1 - 2 * timePulledBack;
            })
            .Multiply();
    }

    private class Day06Part1Tests
    {
        [Test]
        public void TestData()
        {
            // Time:      7  15   30
            // Distance:  9  40  200
            var sut = new Day06Part1();
            Assert.That(sut.Run(new List<(int, int)>
            {
                (7, 9),
                (15, 40),
                (30, 200)
            }), Is.EqualTo(288));
        }

        [Test]
        public void Data()
        {
            // Time:        60     94     78     82
            // Distance:   475   2138   1015   1650
            var sut = new Day06Part1();
            Assert.That(sut.Run(new List<(int, int)>
            {
                (60, 475),
                (94, 2138),
                (78, 1015),
                (82, 1650)
            }), Is.EqualTo(345015));
        }
    }
}