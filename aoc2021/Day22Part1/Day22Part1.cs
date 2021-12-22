using NUnit.Framework;

namespace aoc2021.Day22Part1;

public class Day22Part1
{
    private int Run(string[] data)
    {
        var cuboids = new Dictionary<(int x, int y, int z), bool>();
        const int min = -51;
        const int max = 51;
        int Clamp(int value) => value < min ? min : value > max ? max : value;

        void Turn(int xMin, int xMax, int yMin, int yMax, int zMin, int zMax, bool isOn, bool force = false)
        {
            xMin = Clamp(xMin);
            xMax = Clamp(xMax);
            yMin = Clamp(yMin);
            yMax = Clamp(yMax);
            zMin = Clamp(zMin);
            zMax = Clamp(zMax);
            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    for (var z = zMin; z <= zMax; z++)
                    {
                        var key = (x,y,z);
                        if(force || cuboids.ContainsKey(key))
                            cuboids[key] = isOn;
                    }
                }
            }
        }
        
        Turn(-50, 50, -50, 50, -50, 50, false, true);
        foreach (var row in data)
        {
            var isOn = row.Split(' ')[0] == "on";
            var parts = row.Split(',');
            var xBounds = parts[0].Split('=')[1].Split("..").Select(int.Parse).ToList();
            var yBounds = parts[1].Split('=')[1].Split("..").Select(int.Parse).ToList();
            var zBounds = parts[2].Split('=')[1].Split("..").Select(int.Parse).ToList();
            Turn(xBounds[0], xBounds[1], yBounds[0], yBounds[1], zBounds[0], zBounds[1], isOn);
        }

        return cuboids.Count(x => x.Value);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day22Part1/testdata.txt");
            var sut = new Day22Part1();
            Assert.AreEqual(590784, sut.Run(data));
        }
        
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day22Part1/data.txt");
            var sut = new Day22Part1();
            Assert.AreEqual(582644, sut.Run(data));
        }
    }
}