using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day17Part1;

public class Day17Part1
{
    private int Run(int xMin, int xMax, int yMin, int yMax)
    {
        var maxValue = 10000;
        var possibleY = GetPossibleY();
        foreach (var (highestY, generation) in possibleY)
        {
            if (WillXWork(generation)) return highestY;
        }

        throw new Exception("No match found");

        IEnumerable<(int, int)> GetPossibleY()
        {
            foreach (var yVelInitial in Enumerable.Range(0, maxValue))
            {
                if (TestYVel(maxValue - yVelInitial, out var highestY, out var generation))
                {
                    yield return (highestY, generation);
                }
            }
        }

        bool TestYVel(int yVel, out int highestY, out int generation)
        {
            var yPos = 0;
            highestY = 0;
            generation = 0;
            while (yPos >= yMin)
            {
                generation++;
                yPos += yVel;
                yVel -= 1;
                highestY = Math.Max(highestY, yPos);
                if (yMin <= yPos && yPos <= yMax)
                {
                    return true;
                }
            }

            return false;
        }

        bool WillXWork(int generation)
        {
            foreach (var xVelInitial in Enumerable.Range(0, maxValue))
            {
                if (TestXVel(maxValue - xVelInitial, generation))
                {
                    return true;
                }
            }

            return false;
        }

        bool TestXVel(int xVel, int generation)
        {
            var xPos = 0;
            foreach (var i in Enumerable.Range(0, generation))
            {
                xPos += xVel;
                xVel = xVel > 0 ? xVel - 1 : xVel + 1;
            }

            return xMin <= xPos && xPos <= xMax;
        }
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var sut = new Day17Part1();
            Assert.AreEqual(45, sut.Run(20, 30, -10, -5));
        }
    
        [Test]
        public void Data()
        {
            var sut = new Day17Part1();
            Assert.AreEqual(7626, sut.Run(211, 232, -124, -69));
        }
    }
}