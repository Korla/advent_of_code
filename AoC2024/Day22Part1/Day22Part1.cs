using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day22Part1;

public class Day22Part1
{
    private long Run(string[] data)
    {
        return data
            .Select(int.Parse)
            .Sum(secret => TransformSecret(secret).Take(2000).Last());

        IEnumerable<long> TransformSecret(long secret)
        {
            while (true)
            {
                secret = ((secret ^ (secret * 64)) + 16777216) % 16777216;
                secret = ((secret ^ (secret / 32)) + 16777216) % 16777216;
                yield return secret = ((secret ^ (secret * 2048)) + 16777216) % 16777216;
            }
        }
    }

    private class Day22Part1Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day22Part1().Run(File.ReadAllLines("Day22Part1/testdata.txt")), Is.EqualTo(37327623));

        [Test] public void Data() =>
            Assert.That(new Day22Part1().Run(File.ReadAllLines("Day22Part1/data.txt")), Is.EqualTo(14082561342));
    }
}