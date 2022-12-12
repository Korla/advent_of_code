using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Utils;

namespace AoC2022.Day11Part1;

public class Day11Part1
{
    private record Monkey(Queue<int> Items, Func<int, int> Operation, Func<int, int> Test, List<int> Inspected);

    private Dictionary<string, Func<int,Func<int, int>>> Operations = new()
    {
        {"*", val => old => old * val},
        {"* old", _ => old => old * old},
        {"+", val => old => old + val},
    };

    private int Run(IEnumerable<string> data)
    {
        var monkeys = new List<Monkey>();
        var remainingData = data.ToArray();
        do
        {
            var items = new Queue<int>();
            foreach (var item in remainingData[1].Replace("  Starting items: ", "").Split(", ").Select(int.Parse))
            {
                items.Enqueue(item);
            }
            var operationParts = remainingData[2].Replace("  Operation: new = old ", "").Split(" ");
            var operation = Operations["* old"](0);
            if (operationParts.Last() != "old")
            {
                operation = Operations[operationParts.First()](int.Parse(operationParts.Last()));
            }
            var testValue = int.Parse(remainingData[3].Replace("  Test: divisible by ", ""));
            var trueCase = int.Parse(remainingData[4].Replace("    If true: throw to monkey ", ""));
            var falseCase = int.Parse(remainingData[5].Replace("    If false: throw to monkey ", ""));
            monkeys.Add(new Monkey(items, operation, val => val % testValue == 0 ? trueCase : falseCase, new List<int>()));
        } while ((remainingData = remainingData.Skip(7).ToArray()).Any());
        foreach (var round in Enumerable.Range(0, 20))
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    var item = monkey.Items.Dequeue();
                    monkey.Inspected.Add(item);
                    var newValue = monkey.Operation(item) / 3;
                    var monkeyToThrowTo = monkey.Test(newValue);
                    monkeys[monkeyToThrowTo].Items.Enqueue(newValue);
                }
            }
        }

        return monkeys.Select(m => m.Inspected.Count).OrderDescending().Take(2).Multiply();
    }

    private class Day11Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day11Part1/testdata.txt");
            var sut = new Day11Part1();
            Assert.AreEqual(10605, sut.Run(data));
        }
    
        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day11Part1/data.txt");
            var sut = new Day11Part1();
            Assert.AreEqual(54253, sut.Run(data));
        }
    }
}