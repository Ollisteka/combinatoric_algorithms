using System;
using FluentAssertions;
using NUnit.Framework;

namespace lab3
{
    [TestFixture]
    public class Point_Should
    {
        [TestCase(0,0,0,1,1)]
        [TestCase(0,4,3,0,5)]
        public void GetDistance(int x1, int y1, int x2, int y2, double expectedDistance)
        {
            var p1 = new Point(x1, y1);
            var p2 = new Point(x2, y2);
            p1.DistanceTo(p2).Should().Be(expectedDistance);
        }
    }
    public class Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Point p)
        {
            return Math.Sqrt(Math.Pow(X - p.X, 2) + Math.Pow(Y - p.Y, 2));
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }
    }
}