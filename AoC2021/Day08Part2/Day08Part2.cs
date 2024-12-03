using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2021.Day08Part2;

public class Day08Part2
{
    /*
        2   c  f
        3 a c  f
        4  bcd f
        5 a cde g
        5 a cd fg
        5 ab d fg
        6 abc efg
        6 ab defg
        6 abcd fg
        7 abcdefg

        e 4
        b 6
        d 7
        g 7
        a 8
        c 8
        f 9
     */
    private int Run(IEnumerable<string> data)
    {
        return data
            .Select(row =>
            {
                var lineParts = row.Split(" | ").Select(part => part.Split(" ")).ToList();
                var input = lineParts.First();
                var output = lineParts.Last();

                var digitCounts = input.SelectMany(d => d).GroupBy(d => d).ToList();
                var e = digitCounts.Single(dc => dc.Count() is 4).Key;
                var b = digitCounts.Single(dc => dc.Count() is 6).Key;
                var f = digitCounts.Single(dc => dc.Count() is 9).Key;
                // a -> c, the one not in length 2
                var ac = digitCounts.Where(dc => dc.Count() is 8).ToList();
                var length2 = input.Single(d => d.Length is 2);
                var c = ac.Single(ac => length2.Contains(ac.Key)).Key;
                var a = ac.Single(ac => ac.Key != c).Key;
                // d -> g, the one not in length 4 
                var dg = digitCounts.Where(dg => dg.Count() is 7).ToList();
                var length4 = input.Single(d => d.Length is 4);
                var d = dg.Single(dg => length4.Contains(dg.Key)).Key;
                var g = dg.Single(dg => dg.Key != d).Key;

                var digits = new List<string>
                {
                    new(new char[] { a, b, c, e, f, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { c, f }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, c, d, e, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, c, d, f, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { b, c, d, f }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, b, d, f, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, b, d, e, f, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, c, f }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, b, c, d, e, f, g }.OrderBy(a => a).ToArray()),
                    new(new char[] { a, b, c, d, f, g }.OrderBy(a => a).ToArray()),
                };
                var outputString = string.Join("",
                    output.Select(digitString =>
                        digits.IndexOf(new string(digitString.ToCharArray().OrderBy(a => a).ToArray()))));
                return int.Parse(outputString);
            })
            .Sum();
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day08Part2/testdata.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(61229));
        }

        [Test]
        public void TestData2()
        {
            var data = File.ReadAllLines(@"Day08Part2/testdata2.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(5353));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day08Part2/data.txt");
            var sut = new Day08Part2();
            Assert.That(sut.Run(data), Is.EqualTo(936117));
        }
    }
}