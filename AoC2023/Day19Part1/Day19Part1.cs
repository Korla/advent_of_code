using NUnit.Framework;

namespace AoC2023.Day19Part1;

public class Day19Part1
{
    private Dictionary<string, Func<Dictionary<string, int>, bool>> Workflows = new()
    {
        {"A", _ => true},
        {"R", _ => false}
    };

    private int Run(IEnumerable<string> data)
    {
        var sum = 0;
        var buildingRules = true;
        foreach (var row in data)
        {
            if (string.IsNullOrEmpty(row))
            {
                buildingRules = false;
                continue;
            }

            if (buildingRules)
            {
                var workflowIdAndStringRule = row.Split("{");
                var workflowId = workflowIdAndStringRule.First();
                var stringRule = workflowIdAndStringRule.Last().Substring(0, workflowIdAndStringRule.Last().Length - 1);
                Workflows.Add(workflowId, CreateWorkflow(stringRule));
            }
            else
            {
                var part = row.Substring(1, row.Length - 2).Split(",").Select(p => p.Split("=")).ToDictionary(v => v.First(), v => int.Parse(v.Last()));
                if(Workflows["in"](part))
                {
                    sum += part.Sum(v => v.Value);
                }
            }
        }

        Func<Dictionary<string, int>, bool> CreateWorkflow(string stringRule)
        {
            var rules = stringRule.Split(",");
            return part =>
            {
                foreach (var rule in rules)
                {
                    if (rule.Contains('>'))
                    {
                        var outcomeAndCondition = rule.Split(':');
                        var outcome = outcomeAndCondition.Last();
                        var condition = outcomeAndCondition.First().Split(">");
                        if (part[condition.First()] > int.Parse(condition.Last()))
                        {
                            return Workflows[outcome](part);
                        }
                    }
                    else if (rule.Contains('<'))
                    {
                        var outcomeAndCondition = rule.Split(':');
                        var outcome = outcomeAndCondition.Last();
                        var condition = outcomeAndCondition.First().Split("<");
                        if (part[condition.First()] < int.Parse(condition.Last()))
                        {
                            return Workflows[outcome](part);
                        }
                    }
                    else
                    {
                        return Workflows[rule](part);
                    }
                }

                throw new Exception();
            };
        }

        return sum;
    }
      
    private class Day19Part1Tests
    {
        [Test]
        public void TestData()
        {
            var data = File.ReadAllLines(@"Day19Part1/testdata.txt");
            var sut = new Day19Part1();
            Assert.AreEqual(19114, sut.Run(data));
        }

        [Test]
        public void Data()
        {
            var data = File.ReadAllLines(@"Day19Part1/data.txt");
            var sut = new Day19Part1();
            Assert.AreEqual(0, sut.Run(data));
        }
    }
}