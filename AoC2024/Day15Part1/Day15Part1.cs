using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day15Part1;

public class Day15Part1
{
    private int Run(IEnumerable<string> data)
    {
        var walls = new List<Vector>();
        var boxes = new List<Vector>();
        var robotPosition = Vector.Origo;
        
        foreach (var result in data.SelectMany((row, y) => row.Select((c, x) => (x, y, c))))
        {
            switch (result.c)
            {
                case '#': walls.Add(new Vector(result.x, result.y)); break;
                case 'O': boxes.Add(new Vector(result.x, result.y)); break;
                case '@': robotPosition = new Vector(result.x, result.y); break;
                case '^':
                case '>':
                case 'v':
                case '<':
                    MoveRobot(Vector.Parse(result.c));
                    break;
            }
        }
        // boxes.Print("O");

        return boxes.Sum(b => b.X + b.Y * 100);

        void MoveRobot(Vector direction)
        {
            var nextAffectedPosition = robotPosition;
            var boxesToMove = new List<Vector>();
            while (true)
            {
                nextAffectedPosition = nextAffectedPosition.Add(direction);
                if (walls.Contains(nextAffectedPosition))
                {
                    return;
                }

                if (boxes.Contains(nextAffectedPosition))
                {
                    boxesToMove.Add(nextAffectedPosition);
                }
                else
                {
                    var movedBoxes = boxesToMove.Select(direction.Add);
                    boxes = boxes.Where(box => !boxesToMove.Contains(box)).ToList();
                    boxes.AddRange(movedBoxes);
                    robotPosition = robotPosition.Add(direction);
                    return;
                }
            }
        }
    }

    private class Day15Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day15Part1/testdata.txt");
            var sut = new Day15Part1();
            Assert.That(sut.Run(data), Is.EqualTo(10092));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day15Part1/data.txt");
            var sut = new Day15Part1();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(1463715));
        }
    }
}