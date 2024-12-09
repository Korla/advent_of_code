using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day09Part2;

public class Day09Part2
{
    private record LillaDuvan(int? FileId, int Size, bool attempted = false)
    {
        public override string ToString()
        {
            return string.Join("", Enumerable.Range(0, Size).Select(_ => FileId?.ToString() ?? "."));
        }
    }

    private double Run(IEnumerable<string> data)
    {
        var diskMap = new LinkedList<LillaDuvan>();
        var row = data.First();
        for (var i = 0; i < row.Length; i += 2)
        {
            diskMap.AddLast(new LillaDuvan(i / 2, int.Parse(row[i].ToString())));
            if (i < row.Length - 1)
            {
                var size = int.Parse(row[i + 1].ToString());
                if (size > 0)
                {
                    diskMap.AddLast(new LillaDuvan(null, size));
                }
            }
        }

        Start: ;
        var frontCount = 0;
        for (var fromBack = diskMap.Last; fromBack != null; fromBack = fromBack.Previous)
        {
            frontCount++;
            if (!fromBack.Value.FileId.HasValue || fromBack.Value.attempted) continue;
            fromBack.Value = fromBack.Value with { attempted = true };

            var backCount = diskMap.Count;
            for (var fromFront = diskMap.First; fromFront != null; fromFront = fromFront.Next)
            {
                backCount--;
                if (fromFront.Value.FileId.HasValue || frontCount > backCount || fromFront.Value.Size < fromBack.Value.Size) continue;

                if (fromFront.Value.Size == fromBack.Value.Size)
                {
                    fromFront.Value = fromBack.Value;
                    fromBack.Value = fromBack.Value with { FileId = null};
                    Join(fromBack);
                    goto Start;
                }

                if (fromFront.Value.Size > fromBack.Value.Size)
                {
                    diskMap.AddAfter(fromFront, new LillaDuvan(null, fromFront.Value.Size - fromBack.Value.Size));
                    fromFront.Value = fromBack.Value;
                    fromBack.Value = fromBack.Value with { FileId = null};
                    Join(fromBack);
                    goto Start;
                }
            }
        }
        
        Log(diskMap);
        var lillaDuvans = diskMap
            .SelectMany(node => Enumerable.Range(0, node.Size).Select(_ => node));
        double count = 0;
        double sum = 0;
        foreach (var node in lillaDuvans)
        {
            sum += node.FileId.HasValue ? node.FileId.Value * count : 0;
            count++;
        }

        return sum;

        void Join(LinkedListNode<LillaDuvan> empty)
        {
            if (empty.Next is { Value.FileId: null })
            {
                empty.Value = empty.Value with { Size = empty.Value.Size + empty.Next.Value.Size };
                diskMap.Remove(empty.Next);
            }
            if (empty.Previous is { Value.FileId: null })
            {
                empty.Previous.Value = empty.Previous.Value with { Size = empty.Previous.Value.Size + empty.Value.Size };
                diskMap.Remove(empty);
            }
        }
    }

    private string Log(LinkedList<LillaDuvan> diskMap)
    {
        var s = "";
        foreach (var node in diskMap)
        {
            s += node;
            Console.Write(node);
        }
        Console.WriteLine();
        return s;
    }

    private class Day09Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day09Part2/testdata.txt");
            var sut = new Day09Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2858));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day09Part2/data.txt");
            var sut = new Day09Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(6460170593016));
        }
    }
}