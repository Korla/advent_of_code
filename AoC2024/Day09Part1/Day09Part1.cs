using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day09Part1;

public class Day09Part1
{
    private double Run(IEnumerable<string> data)
    {
        var diskMap = data.First().Select(c => int.Parse(c.ToString())).ToList();
        var fileIdsFromStart = FileIdsFromStart(diskMap);
        using var fileFromEnd = FileIdsFromEnd(diskMap).GetEnumerator();
        fileFromEnd.MoveNext();
        double sum = 0;
        var count = 0;
        var lastYieldedFromStart = (fileId: 0, position: 0);
        foreach (var fileFromStart in fileIdsFromStart)
        {
            var value = fileFromStart.value;
            if (!fileFromStart.value.HasValue)
            {
                value = fileFromEnd.Current.fileId;
                fileFromEnd.MoveNext();
            }
            else
            {
                lastYieldedFromStart = (fileFromStart.fileId, fileFromStart.position);
            }
            sum += value.Value * count++;
            if (lastYieldedFromStart.fileId > fileFromEnd.Current.fileId ||
                lastYieldedFromStart.fileId == fileFromEnd.Current.fileId && lastYieldedFromStart.position + 1 >= fileFromEnd.Current.position)
            {
                break;
            }
        }
        return sum;
    }

    private static IEnumerable<(int? value, int fileId, int position)> FileIdsFromStart(List<int> diskMap)
    {
        var numberOfFiles = (diskMap.Count + 1) / 2;
        for (var fileId = 0; fileId < numberOfFiles; fileId++)
        {
            var fileLength = 0;
            while (fileLength < diskMap[fileId * 2])
            {
                yield return (fileId, fileId, fileLength);
                fileLength++;
            }
            var numberOfFreeSpaces = 0;
            while (numberOfFreeSpaces < diskMap[fileId * 2 + 1])
            {
                yield return (null, fileId, numberOfFreeSpaces);
                numberOfFreeSpaces++;
            }
        }
    }

    private static IEnumerable<(int fileId, int position)> FileIdsFromEnd(List<int> diskMap)
    {
        var numberOfFiles = (diskMap.Count + 1) / 2;
        for (var fileId = numberOfFiles - 1; fileId >= 0; fileId--)
        {
            var fileLength = diskMap[fileId * 2];
            while (fileLength > 0)
            {
                yield return (fileId, fileLength);
                fileLength--;
            }
        }
    }

    private class Day09Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day09Part1/testdata.txt");
            var sut = new Day09Part1();
            Assert.That(sut.Run(data), Is.EqualTo(1928));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day09Part1/data.txt");
            var sut = new Day09Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(6430446922192));
        }
    }
}