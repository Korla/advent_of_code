using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;

namespace aoc2021.Day16Part1;

public class Day16Part1
{
    private long Run(IList<string> data)
    {
        var map = new Dictionary<char, string>
        {
            {'0', "0000"},
            {'1', "0001"},
            {'2', "0010"},
            {'3', "0011"},
            {'4', "0100"},
            {'5', "0101"},
            {'6', "0110"},
            {'7', "0111"},
            {'8', "1000"},
            {'9', "1001"},
            {'A', "1010"},
            {'B', "1011"},
            {'C', "1100"},
            {'D', "1101"},
            {'E', "1110"},
            {'F', "1111"}
        };
        var binaryString = string.Join("", data.First().Select(c => map[c]));
        var current = 0;
        return RecRead();

        long RecRead()
        {
            var version = ReadInt(3);
            var packetTypeId = ReadInt(3);
            var isOperator = packetTypeId != 4;
            if (isOperator)
            {
                if (ReadInt(1) == 0)
                {
                    return version + HandleType15();
                }

                return version + HandleType11();
            }

            ReadLiteralValue();
            return version;
        }

        long HandleType15()
        {
            var lengthOfSubpackets = ReadInt(15);
            var endOfPacket = current + lengthOfSubpackets;

            long total = 0;
            while (current != endOfPacket)
            {
                total += RecRead();
            }
            return total;
        }

        long HandleType11()
        {
            var numberOfSubpackets = ReadInt(11);
            long total = 0;
            for (var i = 0; i < numberOfSubpackets; i++)
            {
                total += RecRead();
            }
            return total;
        }

        long ReadLiteralValue()
        {
            bool continueReading;
            var value = "";
            do
            {
                continueReading = ReadInt(1) == 1;
                value += ReadBinary(4);
            } while (continueReading);

            return Convert.ToInt64(value, 2);
        }

        long ReadInt(int length)
        {
            var readBinary = ReadBinary(length);
            return Convert.ToInt64(readBinary, 2);
        }

        string ReadBinary(int length) => binaryString[current..(current += length)];
    }

    private class Tests
    {
        [Theory]
        [TestCase("D2FE28", 6)]
        [TestCase("38006F45291200", 9)]
        [TestCase("EE00D40C823060", 14)]
        [TestCase("8A004A801A8002F478", 16)]
        [TestCase("620080001611562C8802118E34", 12)]
        [TestCase("C0015000016115A2E0802F182340", 23)]
        [TestCase("A0016C880162017C3686B18A3D4780", 31)]
        public void TestData(string input, long expected)
        {
            var sut = new Day16Part1();
            Assert.AreEqual(expected, sut.Run(new List<string> { input }));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day16Part1/data.txt");
            var sut = new Day16Part1();
            Assert.AreEqual(999, sut.Run(data));
        }
    }
}