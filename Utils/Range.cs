using NUnit.Framework;

namespace Utils;

public record Range(double start, double end);
public static class RangeExtensions
{
    private static bool HasDistance(this Range range) => range.end - range.start != 0;

    private static Range Join(this Range r1, Range r2)
    {
        var bounds = new List<double> { r1.start, r1.end, r2.start, r2.end }
            .OrderBy(a => a)
            .ToList();
        return new Range(bounds.First(), bounds.Last());
    }

    public static IEnumerable<Range> Split(this Range r1, Range r2)
    {
        // no overlap or contained
        if (!r1.Overlaps(r2) || r2.Contains(r1))
        {
            return new List<Range> { r1 }.Where(HasDistance);
        }

        if (r1.Contains(r2))
        {
            return new List<Range>
            {
                new(r1.start, r2.start),
                new(r2.start, r2.end),
                new(r2.end, r1.end)
            }.Where(HasDistance);
        }

        var middle = r1.start < r2.start ? r2.start : r2.end;
        return new List<Range>
        {
            new(r1.start, middle),
            new(middle, r1.end)
        }.Where(HasDistance);
    }

    public static IEnumerable<Range> Simplify(this IEnumerable<Range> ranges)
    {
        var ordered = ranges.OrderBy(r => r.start).ToList();
        var result = new Queue<Range>();
        result.Enqueue(ordered.First());
        foreach (var current in ordered.Skip(1))
        {
            var overlaps = current.Overlaps(result.Last());
            result.Enqueue(
                overlaps ?
                    current.Join(result.Dequeue()) :
                    current
            );
        }

        return result;
    }

    private static bool Overlaps(this Range range, Range other)
    {
        return !(range.end < other.start || range.start > other.end);
    }

    public static bool Contains(this Range range, Range other)
    {
        return range.start <= other.start && range.end >= other.end;
    }

    public static Range Move(this Range range, double distance)
    {
        return new Range(range.start + distance, range.end + distance);
    }

    private class RangeTests
    {
        [TestCase(1, 2, 3, 4, 1, 4)]
        [TestCase(3, 4, 1, 2, 1, 4)]
        [TestCase(1, 4, 2, 3, 1, 4)]
        [TestCase(2, 3, 1, 4, 1, 4)]
        public void JoinTests(double s1, double e1, double s2, double e2, double s3, double e3)
        {
            Assert.That(new Range(s1, e1).Join(new Range(s2, e2)), Is.EqualTo(new Range(s3, e3)));
        }

        [Test]
        public void SplitToOneTests()
        {
            Assert.That(
                new Range(1, 2).Split(new Range(3, 4)),
                Is.EqualTo(new List<Range> { new(1, 2) })
            );
        }

        [TestCase(1, 3, 2, 4, 1, 2, 2, 3)]
        [TestCase(1, 3, 1, 2, 1, 2, 2, 3)]
        public void SplitToTwoTests(double s1, double e1, double s2, double e2, double s3, double e3, double s4, double e4)
        {
            Assert.That(
                new Range(s1, e1).Split(new Range(s2, e2)),
                Is.EqualTo(new List<Range> { new(s3, e3), new(s4, e4) })
            );
        }

        [Test]
        public void SplitToThreeTests()
        {
            Assert.That(
                new Range(1, 4).Split(new Range(2, 3)),
                Is.EqualTo(new List<Range> { new(1, 2), new(2, 3), new(3, 4) })
            );
        }

        [Test]
        public void SimplifyTouchingTests()
        {
            Assert.That(
                new List<Range> { new(1, 2), new(2, 3), new(3, 4) }.Simplify(),
                Is.EqualTo(new List<Range> { new(1, 4) })
            );
        }

        [Test]
        public void SimplifyNoTouchTests()
        {
            Assert.That(
                new List<Range> { new(1, 2), new(3, 4) }.Simplify(),
                Is.EqualTo(new List<Range> { new(1, 2), new(3, 4) })
            );
        }
    }
}