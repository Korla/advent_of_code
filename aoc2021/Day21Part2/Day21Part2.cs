using System.Collections.Concurrent;
using NUnit.Framework;

namespace aoc2021.Day21Part2;

public class Day21Part2
{
    private static readonly (int add, int count)[] PossibleRolls =
        {(3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1)};

    private static readonly Func<(int who, int player1, int player2, int score1, int score2), long[]> MemoizedSolveRec =
        Memoize<(int who, int player1, int player2, int score1, int score2), long[]>(SolveRec);

    private static long[] SolveRec((int who, int player1, int player2, int score1, int score2) input)
    {
        var (who, player1, player2, score1, score2) = input;
        var players = new[] {player1, player2};
        var scores = new[] {score1, score2};

        if (scores[0] >= 21) return new long[] {1, 0};
        if (scores[1] >= 21) return new long[] {0, 1};

        var ans = new long[] {0, 0};

        foreach (var (add, count) in PossibleRolls)
        {
            players[who] = (players[who] + add - 1) % 10 + 1;
            scores[who] += players[who];
            var now = MemoizedSolveRec((1 - who, players[0], players[1], scores[0], scores[1]));
            ans[0] += count * now[0];
            ans[1] += count * now[1];
            scores[who] -= players[who];
            players[who] = (players[who] - add - 1) % 10 + 1;
        }

        return ans;
    }

    private long Run(int p1, int p2)
    {
        var res = MemoizedSolveRec((0, p1, p2, 0, 0));
        return Math.Max(res[0], res[1]);
    }

    private static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> f) where T : notnull
    {
        var cache = new ConcurrentDictionary<T, TResult>();
        return a => cache.GetOrAdd(a, f);
    }

    private class Tests
    {
        [Test]
        public void TestData()
        {
            var sut = new Day21Part2();
            Assert.AreEqual(444356092776315, sut.Run(4, 8));
        }

        [Test]
        public void Data()
        {
            var sut = new Day21Part2();
            Assert.AreEqual(712381680443927, sut.Run(6, 8));
        }
    }
}
