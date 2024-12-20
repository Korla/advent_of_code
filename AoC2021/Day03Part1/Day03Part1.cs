﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day03Part1;

public class Day03Part1
{
    private int Run(IEnumerable<string> data)
    {
        data = data.ToList();
        var values = data
            .Aggregate(
                Enumerable.Range(0, data.First().Length).Select(_ => 0),
                (prev, row) => prev.Zip(row, (result, character) => result + (character == '1' ? 1 : -1))
            )
            .ToList();
        var gamma = new string(values.Select(v => v > 0 ? '1' : '0').ToArray());
        var epsilon = new string(values.Select(v => v < 0 ? '1' : '0').ToArray());
        return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day03Part1/testdata.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(198));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day03Part1/data.txt");
            var sut = new Day03Part1();
            Assert.That(sut.Run(data), Is.EqualTo(2640986));
        }
    }
}