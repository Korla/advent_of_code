using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils;

namespace AoC2024.Day15Part2;

public class Day15Part2
{
    private record Box(Vector Left, Vector Right)
    {
        public Box Move(Vector direction)
        {
            return new Box(Left + direction, Right + direction);
        }
    }

    private int Run(IEnumerable<string> data)
    {
        var walls = new List<Vector>();
        var boxes = new List<Box>();
        var robotPosition = Vector.Origo;
        
        foreach (var result in data.SelectMany((row, y) => row.Select((c, x) => (x, y, c))))
        {
            var x = result.x * 2;
            var y = result.y;
            var content = result.c;
            switch (content)
            {
                case '#':
                    walls.Add(new Vector(x, y));
                    walls.Add(new Vector(x + 1, y));
                    break;
                case 'O': boxes.Add(new Box(new Vector(x, y), new Vector(x + 1, y))); break;
                case '@': robotPosition = new Vector(x, y); break;
                case '^':
                case '>':
                case 'v':
                case '<':
                    MoveRobot(Vector.Parse(content));
                    break;
            }
        }

        // Print(robotPosition, walls, boxes);

        return boxes.Sum(b => b.Left.X + b.Left.Y * 100);

        void MoveRobot(Vector direction)
        {
            List<Vector> nextAffectedPositions = [robotPosition];
            var boxesToMove = new List<Box>();
            while (true)
            {
                nextAffectedPositions = nextAffectedPositions.Select(direction.Add).ToList();
                if (walls.Intersect(nextAffectedPositions).Any())
                {
                    return;
                }
                
                var newBoxesToMove = boxes.Where(b => nextAffectedPositions.Contains(b.Left) || nextAffectedPositions.Contains(b.Right)).ToList();
                if (newBoxesToMove.Count != 0)
                {
                    nextAffectedPositions = newBoxesToMove
                        .Where(b => !boxesToMove.Contains(b))
                        .SelectMany(b => new[] { b.Left, b.Right })
                        .ToList();
                    boxesToMove.AddRange(newBoxesToMove);
                    boxesToMove = boxesToMove.Distinct().ToList();
                }
                else
                {
                    if (boxesToMove.Any())
                    {
                        foreach (var box in boxesToMove)
                        {
                            boxes.Remove(box);
                        }
                        var movedBoxes = boxesToMove.Select(b => b.Move(direction));
                        boxes.AddRange(movedBoxes);
                    }
                    robotPosition = robotPosition.Add(direction);
                    return;
                }
            }
        }
    }

    private static void Print(Vector robotPosition, List<Vector> walls, List<Box> boxes)
    {
        var printables = new List<(Vector vector, string value)> { (robotPosition, "@") };
        printables.AddRange(walls.Select(w => (w, "#")));
        printables.AddRange(boxes.SelectMany(b => new (Vector vector, string value)[]{(b.Left, "["), (b.Right, "]")}));
        printables.Print();
    }

    private class Day15Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day15Part2/testdata.txt");
            var sut = new Day15Part2();
            Assert.That(sut.Run(data), Is.EqualTo(9021));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day15Part2/data.txt");
            var sut = new Day15Part2();
            var actual = sut.Run(data);
            Assert.That(actual, Is.EqualTo(1481392));
        }
    }
}