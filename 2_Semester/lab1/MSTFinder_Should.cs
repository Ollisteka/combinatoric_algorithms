using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace lab1
{
    [TestFixture]
    public class MSTFinder_Should
    {
        private string[] labArgs = {
            "4",
            "2 5 3 15 0",
            "1 5 3 0 0",
            "1 15 2 0 4 25 0",
            "3 25 0"
        };

        private string[] labResult = {
            "2 5 0",
            "1 5 3 0 0",
            "2 0 4 25 0",
            "3 25 0",
            "30"
        };

        private MSTFinder sut;

        [SetUp]
        public void SetUp()
        {
            sut = new MSTFinder(labArgs);
        }

        [Test]
        public void Init()
        {
            sut.Vertices.Should().BeEquivalentTo(new List<int> {1, 2, 3, 4});
            sut.Edges.Should().BeEquivalentTo(new List<Edge>
            {
                new Edge(1, 2, 5),
                new Edge(1, 3, 15),
                new Edge(2, 3, 0),
                new Edge(3, 4, 25)
            });
        }

        [Test]
        public void SortEdges()
        {
            sut.GetSortedEdgesQueue().Should().BeInAscendingOrder(edge => edge.Weight);
        }

        [Test]
        public void GetMinimalSpanningTree()
        {
            sut.FindMinimalSpanningTree().Should().BeEquivalentTo(new List<Edge>
            {
                new Edge(1, 2, 5),
                new Edge(2, 3, 0),
                new Edge(3, 4, 25)
            });
        }

        [Test]
        public void PrintMST()
        {
            var mst = sut.FindMinimalSpanningTree();
            sut.PrintMST(mst).Should().BeEquivalentTo(labResult);
        }
    }
}