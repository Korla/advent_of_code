using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace AoC2022.Day11Part2;

public class Day11Part2
{
    private record Monkey(Queue<long> Items, Func<long, long> Operation, Func<long, int> Test, List<long> Inspected);

    private Dictionary<string, Func<long, Func<long, long>>> Operations = new()
    {
        {"*", val => old => old * val},
        {"* old", _ => old => old * old},
        {"+", val => old => old + val},
    };

    private long Run(IEnumerable<string> data)
    {
        var modulo = 1;
        var monkeys = new List<Monkey>();
        var remainingData = data.ToArray();
        do
        {
            var items = new Queue<long>();
            foreach (var item in remainingData[1].Replace("  Starting items: ", "").Split(", ").Select(long.Parse))
            {
                items.Enqueue(item);
            }
            var operationParts = remainingData[2].Replace("  Operation: new = old ", "").Split(" ");
            var operation = Operations["* old"](0);
            if (operationParts.Last() != "old")
            {
                operation = Operations[operationParts.First()](long.Parse(operationParts.Last()));
            }
            var testValue = int.Parse(remainingData[3].Replace("  Test: divisible by ", ""));
            modulo *= testValue;
            var trueCase = int.Parse(remainingData[4].Replace("    If true: throw to monkey ", ""));
            var falseCase = int.Parse(remainingData[5].Replace("    If false: throw to monkey ", ""));
            monkeys.Add(new Monkey(items, operation, val => val % testValue == 0 ? trueCase : falseCase, new List<long>()));
        } while ((remainingData = remainingData.Skip(7).ToArray()).Any());
        foreach (var round in Enumerable.Range(0, 10000))
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    var item = monkey.Items.Dequeue();
                    monkey.Inspected.Add(item);
                    var newValue = monkey.Operation(item) % modulo;
                    var monkeyToThrowTo = monkey.Test(newValue);
                    monkeys[monkeyToThrowTo].Items.Enqueue(newValue);
                }
            }
        }

        var a = monkeys.Select(m => m.Inspected.Count).OrderDescending().ToArray();
        return a[0] * (long)a[1];
    }

    private class Day11Part2Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day11Part2/testdata.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(2713310158));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day11Part2/data.txt");
            var sut = new Day11Part2();
            Assert.That(sut.Run(data), Is.EqualTo(13119526120));
        }
    }
}