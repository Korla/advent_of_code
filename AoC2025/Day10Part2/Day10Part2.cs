using Microsoft.Z3;

namespace AoC2025.Day10Part2;

public class Day10Part2
{
    private long Run(IEnumerable<string> d)
    {
        return d.Sum(row =>
        {
            var parts = row.Split(' ');
            var target = parts.Last()
                .Substring(1, parts.Last().Length - 2).Split(',').Select(int.Parse).ToArray();
            var buttons = parts.Skip(1).SkipLast(1)
                .Select(b => b.Substring(1, b.Length - 2).Split(',').Select(int.Parse).ToArray()).ToList();
            return SolveLinear(target, buttons);
        });
    }

    private static int SolveLinear(int[] target, List<int[]> buttons)
    {
        using var ctx = new Context();
        using var opt = ctx.MkOptimize();

        var buttonPresses = new IntExpr[buttons.Count];

        for (var j = 0; j < buttons.Count; j++)
        {
            buttonPresses[j] = ctx.MkIntConst($"y{j}");
            opt.Add(ctx.MkGe(buttonPresses[j], ctx.MkInt(0)));
        }

        for (var i = 0; i < target.Length; i++)
        {
            ArithExpr sum = ctx.MkInt(0);
            for (var j = 0; j < buttons.Count; j++)
            {
                if (Array.Exists(buttons[j], idx => idx == i))
                {
                    sum = ctx.MkAdd(sum, buttonPresses[j]);
                }
            }
            opt.Add(ctx.MkEq(sum, ctx.MkInt(target[i])));
        }

        var total = buttonPresses.Aggregate((ArithExpr)ctx.MkInt(0), (current, xi) => ctx.MkAdd(current, xi));

        opt.MkMinimize(total);
        return opt.Check() != Status.SATISFIABLE ?
            throw new Exception("Not satisfiable") :
            ((IntNum)opt.Model.Evaluate(total)).Int;
    }

    private class Day10Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines("Day10Part2/testdata.txt");
            var sut = new Day10Part2();
            Assert.That(sut.Run(data), Is.EqualTo(33));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines("Day10Part2/data.txt");
            var sut = new Day10Part2();
            Assert.That(sut.Run(data), Is.EqualTo(21824));
        }
    }
}