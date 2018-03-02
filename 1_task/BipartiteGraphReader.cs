using System.Collections.Generic;
using System.Linq;

namespace _1_task
{
	public class BipartiteGraphReader
	{
		public readonly List<List<int>> Nodes = new List<List<int>>();
		public int[] Nums;
		public List<int> PartEven = new List<int>();
		public List<int> PartOdd = new List<int>();

		public BipartiteGraphReader(string[] args)
		{
			var pointer = 1;
			for (var i = 0; i < args.Length; i++)
				Nodes.Add(new List<int>());

			foreach (var line in args.Skip(1))
			{
				var connectsdNodes = line.Split(' ').Where(x => x != "0").Select(x => int.Parse(x));
				foreach (var node in connectsdNodes)
					Nodes[pointer].Add(node);
				pointer++;
			}
			var nodesCount = int.Parse(args[0]);

			Nums = new int[nodesCount + 1];
			Nums[0] = int.MinValue;
		}

		private void BFS(int startNode = 1)
		{
			var queue = new Queue<int>();
			queue.Enqueue(startNode);
			Nums[startNode] = 1;
			while (queue.Count != 0)
			{
				var node = queue.Dequeue();
				foreach (var incindentNode in Nodes[node])
					if (Nums[incindentNode] == 0)
					{
						queue.Enqueue(incindentNode);
						Nums[incindentNode] = Nums[node] + 1;
					}
			}
		}

		private static void TryAddValue(Dictionary<int, List<int>> dict, int key, int val)
		{
			if (dict.ContainsKey(key))
				dict[key].Add(val);
			else
				dict.Add(key, new List<int> {val});
		}

		private bool HasAdjacentNodesOfSameLevel(Dictionary<int, List<int>> dict)
		{
			foreach (var pair in dict)
			foreach (var node in dict[pair.Key])
			{
				var allOtherNodes = dict[pair.Key].Where(x => x != node);
				var adjacentNodes = Nodes[node];
				if (allOtherNodes.Intersect(adjacentNodes).Count() != 0)
					return true;
			}
			return false;
		}

		public bool IsGraphBipartite()
		{
			BFS(1);
			if (Nums.Count(element => element == 0) != 0)
				return false;
			var even = new Dictionary<int, List<int>>();
			var odd = new Dictionary<int, List<int>>();
			for (var i = 1; i < Nums.Length; i++)
				TryAddValue(Nums[i] % 2 == 0 ? even : odd, Nums[i], i);


			if (HasAdjacentNodesOfSameLevel(even) || HasAdjacentNodesOfSameLevel(odd))
				return false;
			PartEven = even.SelectMany(x => x.Value).ToList();
			PartOdd = odd.SelectMany(x => x.Value).ToList();
			return true;
		}
	}
}