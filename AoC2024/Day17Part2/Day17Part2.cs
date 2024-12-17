using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day17Part2;

public class Day17Part2
{
    private record State(long RegisterA, long RegisterB, long RegisterC, IEnumerable<int> Result, int InstructionPointer);

    private long Run(IEnumerable<string> data)
    {
        var dataArray = data.ToArray();
        var registerA = int.Parse(dataArray[0][12..]);
        var registerB = int.Parse(dataArray[1][12..]);
        var registerC = int.Parse(dataArray[2][12..]);
        var program = dataArray[4][9..].Split(",").Select(int.Parse).ToList();

        var powerLength = 1;
        while (RunProgramForNumber(MakeNumber(Enumerable.Range(0, powerLength).Select(_ => 1))).Result.Count() < program.Count)
        {
            powerLength++;
        }

        var results = new List<long>();
        var queue = new Queue<List<int>>();
        queue.Enqueue([]);
        while (queue.Count != 0)
        {
            var current = queue.Dequeue();
            var zeroes = Enumerable.Range(0, powerLength - current.Count - 1).Select(_ => 0).ToList();
            foreach (var i in Enumerable.Range(0, 9))
            {
                var potentialCorrectDigits = current.Concat([i]).ToList();
                var potentialDigits = potentialCorrectDigits.Concat(zeroes).ToList();
                var potentialNumber = MakeNumber(potentialDigits);
                var potentialResult = RunProgramForNumber(potentialNumber);
                var resultToSkip = potentialResult.Result.Count() - potentialCorrectDigits.Count;

                if (!potentialResult.Result.Skip(resultToSkip).Take(potentialCorrectDigits.Count)
                        .SequenceEqual(program.Skip(resultToSkip).Take(potentialCorrectDigits.Count))) continue;

                if (potentialCorrectDigits.Count == program.Count)
                {
                    results.Add(potentialNumber);
                }
                else
                {
                    queue.Enqueue(potentialCorrectDigits);
                }
            }
        }

        return results.Min();

        State RunProgramForNumber(double a)
        {
            return RunProgram(program, new State((long)a, registerB, registerC, [], 0));
        }
        
        long MakeNumber(IEnumerable<int> places)
        {
            places = places.Reverse();
            return (long)places.Select((p, i) => p * Math.Pow(8, i)).Sum();
        }
    }

    private static State RunProgram(List<int> program, State state)
    {
        var visited = new HashSet<State>();
        while (!visited.Contains(state))
        {
            visited.Add(state);
            if (state.InstructionPointer >= program.Count - 1)
            {
                break;
            }
            var opCode = program[state.InstructionPointer];
            var literalOperand = program[state.InstructionPointer + 1];
            switch (opCode)
            {
                case 0: // adv
                    state = state with { RegisterA = (long)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
                    break;
                case 1: // bxl
                    state = state with { RegisterB = state.RegisterB ^ literalOperand };
                    break;
                case 2: // bst
                    state = state with { RegisterB = ComboOperand(literalOperand) % 8 };
                    break;
                case 3: // jnz
                    if (state.RegisterA == 0) continue;
                    state = state with { InstructionPointer = literalOperand };
                    continue;
                case 4: // bxc
                    state = state with { RegisterB = state.RegisterB ^ state.RegisterC };
                    break;
                case 5: // out
                    state = state with { Result = state.Result.Concat([(int)(ComboOperand(literalOperand) % 8)]) };
                    break;
                case 6: // bdv
                    state = state with { RegisterB = (long)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
                    break;
                case 7: // cdv
                    state = state with { RegisterC = (long)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
                    break;
            }

            state = state with { InstructionPointer = state.InstructionPointer + 2 };
        }

        return state;
        
        long ComboOperand(int literalOperand) =>
            literalOperand switch
            {
                0 => literalOperand,
                1 => literalOperand,
                2 => literalOperand,
                3 => literalOperand,
                4 => state.RegisterA,
                5 => state.RegisterB,
                6 => state.RegisterC,
                _ => throw new Exception("Invalid opcode"),
            };
    }

    private class Day17Part2Tests
    {
        [Test] public void TestData() =>
            Assert.That(new Day17Part2().Run(File.ReadAllLines("Day17Part2/testdata.txt")), Is.EqualTo(117440));

        [Test] public void Data() =>
            Assert.That(new Day17Part2().Run(File.ReadAllLines("Day17Part2/data.txt")), Is.EqualTo(202991746427434));
    }
}