using System.Collections.Generic;
using System.IO;
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
        private string[] NoWayArgs =
        {
            "4",
            "0",
            "1 25 3 0 0", "1 4 0",
            "3 7 0", "2", "4"
        };
        private string[] AnotherWayArgs =
        {
            "4",
            "0",
            "1 25 3 0 0", "1 4 0",
            "3 7 0", "1", "2"
        };

        private string[] CrachingArgs =
        {
            "9",
            "0", //1
            "1  1  0",  //2
            "1  5  2  2  0", //3
            "1  3  3 10  0", //4
            "2  2  0", //5
            "2 10  3 10  4  1  5 15  0", //6
            "4  2  6 10  0", //7
            "5 12  6  2  7 15  0", //8
            "0", //9
            "1",
            "8",

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
        public void Crash()
        {
            pathFinder = new NetworkPathFinder(CrachingArgs);
            pathFinder.GetWay().Should().BeEquivalentTo(new List<int> {1,3,6,7,8});
             pathFinder.Cost.Should().Be(7500);
        }

        [Test]
        public void Input5()
        {
            var args = File.ReadAllLines(@"C:\Users\Olga\source\repos\combinatoric_algorithms\3_task\bin\Debug\In5.txt");
            pathFinder = new NetworkPathFinder(args);
            var way = pathFinder.GetWay();
            way.Should().BeNull();
            pathFinder.Cost.Should().Be(1);
        }

        [Test]
        public void Should_FindWay_When_ThereIsOne()
        {
            var wayStack = pathFinder.GetWay();
            var way = new List<int>();
            while (wayStack.Count!=0)
                way.Add(wayStack.Pop());
            way.Should().BeEquivalentTo(new List<int>{1,3,4});
            pathFinder.Cost.Should().Be(28);
        }
        
        [Test]
        public void Should_NotFindWay_When_ThereNone()
        {
            var finder = new NetworkPathFinder(NoWayArgs);
            finder.GetWay().Should().BeNull();
            finder.Cost.Should().Be(1);
        }
        

        [Test]
        public void Should_FindShortesWay()
        {
            var finder = new NetworkPathFinder(AnotherWayArgs);
            var wayStack = finder.GetWay();
            var way = new List<int>();
            while (wayStack.Count != 0)
                way.Add(wayStack.Pop());
            way.Should().BeEquivalentTo(new List<int> { 1, 2 });
            finder.Cost.Should().Be(25);
        }
    }
}