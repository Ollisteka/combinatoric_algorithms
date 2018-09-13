using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace _2_task
{
	[TestFixture]
	internal class PathFinderShould
	{
		private readonly string[] args =
		{
			"4",
			"5",

			"1 1 1 1 1",

			"1 0 1 0 1",

			"1 0 0 0 1",

			"1 1 1 1 1",

			"2 2",

			"2 4"
		};

		private readonly string[] testArgs =
			{"12",
				"15",
				"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1",
				"1 0 0 0 0 0 0 0 0 1 0 0 0 0 1",
				"1 0 0 0 0 0 0 0 1 0 0 0 0 0 1",
				"1 0 0 0 0 0 0 1 0 1 0 1 0 1 1",
				"1 0 0 0 0 0 1 0 1 0 1 0 1 0 1",
				"1 0 0 0 0 1 0 0 0 0 0 0 0 0 1",
				"1 0 0 0 1 0 0 1 0 0 0 0 0 0 1",
				"1 0 0 1 0 1 0 1 1 1 1 1 1 1 1",
				"1 0 0 0 0 0 0 0 0 0 0 0 0 0 1",
				"1 1 0 0 1 1 0 0 0 0 0 0 0 0 1",
				"1 0 0 0 1 0 0 0 0 0 0 0 0 0 1",
				"1 1 1 1 1 1 1 1 1 1 1 1 1 1 1",
				"9 3",
				"6 13",};

	private List<Point> way = new List<Point>
		{
			new Point(2, 2),

			new Point(3, 2),

			new Point(3, 3),

			new Point(3, 4),

			new Point(2, 4)
		};

		[Test]
		public void TestInput()
		{
			var finder = new PathFinder(testArgs);
			finder.CanFindWay().Should().BeTrue();
			var result = finder.way;
		}
		[Test]
		public void DoSomething_WhenSomething()
		{
			var finder = new PathFinder(args);
			finder.CanFindWay().Should().BeTrue();
			finder.way.Should().BeEquivalentTo(way);
		}
	}
}