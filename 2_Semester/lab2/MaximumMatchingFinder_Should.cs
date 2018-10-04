using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace lab2
{
    [TestFixture]
    public class MaximumMatchingFinder_Should
    {
        private string[] testArgs =
        {
            "4 4",
            "1 1 1 1",
            "0 1 1 1",
            "0 0 1 1",
            "0 0 0 1",
        };

        private string[] myArgs =
        {
            "4 4",
            "0 1 0 0",
            "0 1 1 1",
            "1 0 0 0",
            "0 0 0 1",
        };


        private string[] expectedAnswer =
        {
            "1",
            "2",
            "3",
            "4",
        };
        private string[] myExpectedAnswer =
        {
            "2",
            "3",
            "1",
            "4",
        };

        private MaximumMatchingFinder sut;

        [SetUp]
        public void SetUp()
        {
            sut = new MaximumMatchingFinder(testArgs);
        }

        [Test]
        public void InitSizes()
        {
            sut.MaxX.Should().Be(4);
            sut.MaxY.Should().Be(4);
            sut.Sink.Should().Be(9);
            sut.Vertices.First().Should().Be(sut.Source);
            sut.Vertices.Last().Should().Be(sut.Sink);
        }

        [Test]
        public void FlattenYCooords()
        {
            sut.GetFlatVerticeForY(1).Should().Be(5);
            sut.GetFlatVerticeForY(2).Should().Be(6);
            sut.GetFlatVerticeForY(3).Should().Be(7);
            sut.GetFlatVerticeForY(4).Should().Be(8);
        }

        [Test]
        public void ThroughputFromSourceToX_Should_BeOne()
        {
            for (int v = 1; v <= sut.MaxX; v++)
                sut.Throughput[sut.Source, v].Should().Be(1);
        }

        [Test]
        public void ThroughputFromSourceToY_Should_BeZero()
        {
            for (int v = sut.MaxX + 1; v <= sut.GetFlatVerticeForY(sut.MaxY); v++)
                sut.Throughput[sut.Source, v].Should().Be(0);
        }


        [Test]
        public void ThroughputFromXToSink_Should_BeZero()
        {
            for (int v = 1; v <= sut.MaxX; v++)
                sut.Throughput[v, sut.Sink].Should().Be(0);
        }

        [Test]
        public void ThroughputFromYToSink_Should_BeOne()
        {
            for (int v = sut.MaxX + 1; v <= sut.GetFlatVerticeForY(sut.MaxY); v++)
                sut.Throughput[v, sut.Sink].Should().Be(1);
        }

        [Test]
        public void FindMaxFlow()
        {
            sut.FindMaxFlow();
            sut.MaxFlow.Should().Be(4);
            sut.GetMatching().Should().BeEquivalentTo(expectedAnswer);
        }

        [Test]
        public void MyArgs()
        {
            sut = new MaximumMatchingFinder(myArgs);
            sut.FindMaxFlow();
            sut.GetMatching().Should().BeEquivalentTo(myExpectedAnswer);
        }
    }
}