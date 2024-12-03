using System;
using NUnit.Framework;

namespace AoC2021.Day17Part2;

public class Day17Part2
{
    private int Run(int xMin, int xMax, int yMin, int yMax)
    {
        var maxValue = 1000;
        var count = 0;
        for (var y = yMin - 1; y < maxValue; y++)
        {
            for (var x = 0; x < xMax + 1; x++)
            {
                if (CheckIfHits(x, y))
                {
                    count++;
                }
            }
        }

        return count;

        bool CheckIfHits(int xVel, int yVel)
        {
            var yPos = 0;
            var xPos = 0;
            while (yPos >= yMin && xPos <= xMax)
            {
                xPos += xVel;
                yPos += yVel;
                yVel -= 1;
                xVel = Math.Max(xVel - 1, 0);
                if (xMin <= xPos && xPos <= xMax && yMin <= yPos && yPos <= yMax)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var sut = new Day17Part2();
            Assert.That(sut.Run(20, 30, -10, -5), Is.EqualTo(112));
        }

        [Test]
        public void Data()
        {
            var sut = new Day17Part2();
            Assert.That(sut.Run(211, 232, -124, -69), Is.EqualTo(2032));
        }
    }
}