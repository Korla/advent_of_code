﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2023.Day01Part1;

public class Day01Part1
{
    private int Run(IEnumerable<string> data)
    {
        return data
           .Sum(line =>
               {
                   var integers = line
                       .Where(c => int.TryParse(c.ToString(), out _))
                       .ToList();
                   return int.Parse($"{integers.First()}{integers.Last()}");
               }
           );
    }

    private class Day01Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day01Part1/testdata.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(142));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day01Part1/data.txt");
            var sut = new Day01Part1();
            Assert.That(sut.Run(data), Is.EqualTo(54927));
        }
    }
}