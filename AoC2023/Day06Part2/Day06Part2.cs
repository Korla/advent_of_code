using NUnit.Framework;

namespace AoC2023.Day06Part2;

public class Day06Part2
{
    // Time:      7  15   30
    // Distance:  9  40  200
    // Time:        60     94     78     82
    // Distance:   475   2138   1015   1650
    private double Run(double time, double distance)
    {
        var timePulledBack = 0;
        while (true)
        {
            var distanceRun = timePulledBack * (time - timePulledBack);
            if (distanceRun > distance)
            {
                return time + 1 - 2 * timePulledBack;
            }
            timePulledBack++;
        }
    }

    private class Day06Part2Tests
    {
        [Test]
        public void TestData()
        {
            // Time:      7  15   30
            // Distance:  9  40  200
            var sut = new Day06Part2();
            Assert.That(sut.Run(71530, 940200), Is.EqualTo(71503));
        }

        [Test]
        public void Data()
        {
            // Time:        60     94     78     82
            // Distance:   475   2138   1015   1650
            var sut = new Day06Part2();
            Assert.That(sut.Run(60947882, 475213810151650), Is.EqualTo(42588603));
        }
    }
}