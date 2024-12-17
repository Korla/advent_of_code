using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2024.Day17Part1;

public class Day17Part1
{
    record State(int RegisterA, int RegisterB, int RegisterC, IEnumerable<int> Result, int InstructionPointer);

    private string Run(IEnumerable<string> data)
    {
        var dataArray = data.ToArray();
        var registerA = int.Parse(dataArray[0][12..]);
        var registerB = int.Parse(dataArray[1][12..]);
        var registerC = int.Parse(dataArray[2][12..]);
        var program = dataArray[4][9..].Split(",").Select(int.Parse).ToList();

        var state = RunProgram(program, new State(registerA, registerB, registerC, [], 0));
        return string.Join(",", state.Result);
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
                    state = state with { RegisterA = (int)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
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
                    state = state with { Result = state.Result.Concat([ComboOperand(literalOperand) % 8]) };
                    break;
                case 6: // bdv
                    state = state with { RegisterB = (int)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
                    break;
                case 7: // cdv
                    state = state with { RegisterC = (int)(state.RegisterA / Math.Pow(2, ComboOperand(literalOperand))) };
                    break;
            }

            state = state with { InstructionPointer = state.InstructionPointer + 2 };
        }

        return state;
        
        int ComboOperand(int literalOperand) =>
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

    private class Day17Part1Tests
    {
        [Test] public void Test1() => Assert.That(RunProgram([2, 6], new State(0, 0, 9, [], 0)).RegisterB, Is.EqualTo(1));
        [Test] public void Test2() => Assert.That(RunProgram([5,0,5,1,5,4], new State(10, 0, 0, [], 0)).Result, Is.EqualTo(new[] { 0,1,2 }));
        [Test] public void Test3() => Assert.That(RunProgram([0,1,5,4,3,0], new State(2024, 0, 0, [], 0)).Result, Is.EqualTo(new[] { 4,2,5,6,7,7,7,7,3,1,0 }));
        [Test] public void Test4() => Assert.That(RunProgram([1,7], new State(0, 29, 0, [], 0)).RegisterB, Is.EqualTo(26));
        [Test] public void Test5() => Assert.That(RunProgram([4,0], new State(0, 2024, 43690, [], 0)).RegisterB, Is.EqualTo(44354));

        [Test] public void TestData() =>
            Assert.That(new Day17Part1().Run(File.ReadAllLines("Day17Part1/testdata.txt")), Is.EqualTo("4,6,3,5,6,3,5,2,1,0"));

        [Test] public void Data() =>
            Assert.That(new Day17Part1().Run(File.ReadAllLines("Day17Part1/data.txt")), Is.EqualTo("7,4,2,0,5,0,5,3,7"));
    }
}