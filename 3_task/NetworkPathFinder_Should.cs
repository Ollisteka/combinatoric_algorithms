using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace _3_task
{
    internal class NetworkPathFinder_Should
    {
        private string[] ExampleArgs =
        {
            "4",
            "0",
            "1 25 3 0 0", "1 4 0",
            "3 7 0", "1", "4"
        };

        private NetworkPathFinder pathFinder;

        [SetUp]
        public void SetUp()
        {
            pathFinder = new NetworkPathFinder(ExampleArgs);
        }


        [Test]
        public void ShouldBeInitializedCorrectly()
        {
            pathFinder.Start.Should().Be(1);
            pathFinder.Finish.Should().Be(4);
            pathFinder.NodeCount.Should().Be(4);
        }

        [Test]
        public void ShouldFindWay()
        {
            var wayStack = pathFinder.GetWay();
            var way = new List<int>();
            while (wayStack.Count!=0)
                way.Add(wayStack.Pop());
            way.Should().BeEquivalentTo(new List<int>{1,3,4});

        }
    }
}