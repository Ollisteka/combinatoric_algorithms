using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace _1_task
{
	[TestFixture]
	internal class BipaertiteGraphShould
	{
		private BipartiteGraphReader biGraphReader;
		private BipartiteGraphReader noBiGraphReader;
		private readonly string[] notBipartiteArgs = {"4", "2 3 4 0", "1 3 4 0", "1 2 4 0", "1 2 3 0"};
		private readonly string[] bipartiteArgs = {"4", "2 4 0", "1 3 0", "2 4 0", "1 3 0"};

		private readonly List<List<int>> notBipartiteNodes =
			new List<List<int>>
			{
				new List<int>(),
				new List<int> {2, 3, 4},
				new List<int> {1, 3, 4},
				new List<int> {1, 2, 4},
				new List<int> {1, 2, 3}
			};

		private readonly List<List<int>> bipartiteNodes =
			new List<List<int>>
			{
				new List<int>(),
				new List<int> {2, 4},
				new List<int> {1, 3},
				new List<int> {2, 4},
				new List<int> {1, 3}
			};

		[SetUp]
		public void SetUp()
		{
			noBiGraphReader = new BipartiteGraphReader(notBipartiteArgs);
			biGraphReader = new BipartiteGraphReader(bipartiteArgs);
		}

		[Test]
		public void ReadArgs_When_NotBipartite()
		{

			noBiGraphReader.Nodes.Count.Should().Be(int.Parse(notBipartiteArgs[0]) + 1);
			noBiGraphReader.Nodes.Should().BeEquivalentTo(notBipartiteNodes);

			noBiGraphReader.Nums.Length.Should().Be(int.Parse(notBipartiteArgs[0]) + 1);
		}

		[Test]
		public void ReadArgs_When_Bipartite()
		{

			biGraphReader.Nodes.Count.Should().Be(int.Parse(bipartiteArgs[0]) + 1);
			biGraphReader.Nodes.Should().BeEquivalentTo(bipartiteNodes);

			biGraphReader.Nums.Length.Should().Be(int.Parse(bipartiteArgs[0]) + 1);
		}

		[Test]
		public void IsBiGraph_When_ItIs()
		{
			biGraphReader.IsGraphBipartite().Should().BeTrue();
			biGraphReader.PartEven.Should().BeEquivalentTo(new List<int>() {2, 4});
			biGraphReader.PartOdd.Should().BeEquivalentTo(new List<int>() {1, 3});
		}
		[Test]
		public void NotBiGraph_When_ItIsNot()
		{
			noBiGraphReader.IsGraphBipartite().Should().BeFalse();
			noBiGraphReader.PartEven.Should().BeEmpty();
			noBiGraphReader.PartOdd.Should().BeEmpty();
		}

		[Test]
		public void TestBiggerBigraph()
		{
			var reader = new BipartiteGraphReader(new []{ "8", "2 4 0", "1 3 0", "2 4 0", "1 3 5 0", "6 0", "7 8 0", "0", "0" });
			reader.IsGraphBipartite().Should().BeTrue();
		}
	}
}