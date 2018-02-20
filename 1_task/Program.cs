using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApplication2
{
	public enum Color
	{
		Pink,
		Black
	}

	public static class ColorExtensions
	{
		public static Color Swap(this Color color)
		{
			return (Color) ((int) (color + 1) % 2);
		}
	}

	public class Node
	{
		public Color Color;
		public bool Visited;
		public int Value;
	}
	internal class Program
	{
		private static readonly List<List<int>> Nodes = new List<List<int>>();

		private static readonly HashSet<int> Visited = new HashSet<int>();
		private static Dictionary<Color, List<int>> Colors = new Dictionary<Color, List<int>>() {{Color.Black, new List<int>()},{Color.Pink, new List<int>()}};

		private static void Main(string[] args)
		{
			args = new[] {"4", "2 0", "1 3 0", "2 4 0", "3 5 0", "5 6 0"};
			ReadArgs(args);
			var success = IsGraphBipartite();
			if (!success)
				File.WriteAllText("out.txt", "N");
			else
				WriteParts();
		}

		private static void WriteParts()
		{
//			foreach (var color in Colors.Keys)
//			{
//				Colors[color].Sort();
//				Colors[color].Add(0);
//			}
//			var firstColor = Colors[Color.Black][0] < Colors[Color.Pink][0] ? Color.Black : Color.Pink;
//			var firstRow = string.Join(" ", Colors[firstColor].Select(x => x.ToString()));
//			var secondRow = string.Join(" ", Colors[firstColor.Swap()].Select(x => x.ToString()));
//			var result = new List<string> {"Y", firstRow, secondRow};
//			File.WriteAllLines("out.txt", result);
		}

		public static void ReadArgs(string[] args)
		{
			var pointer = 1;
			for (int i = 0; i < args.Length; i++)
			{
				Nodes.Add(new List<int>());
			}
			//foreach (var line in File.ReadLines(args[0]).Skip(1))
			foreach (var line in args.Skip(1))
			{
				var connectsdNodes = line.Split(' ').Where(x => x != "0").Select(x => int.Parse(x));
				foreach (var node in connectsdNodes)
					Nodes[pointer].Add(node);
				pointer++;
			}
		}

		public static bool IsGraphBipartite(Color startColor= Color.Black, int startNode=1)
		{
		 	var queue = new Queue<int>();
			queue.Enqueue(startNode);
			while (queue.Count != 0)
			{
				var node = queue.Dequeue();
				if (Visited.Contains(node))
					continue;
				Visited.Add(node);
				Colors[startColor].Add(node);

				foreach (var incindentNodes in Nodes[node])
				{
					if ((Colors[Color.Black].Contains(incindentNodes) && startColor == Color.Black)
						|| (Colors[Color.Pink].Contains(incindentNodes) && startColor == Color.Pink))
						return false;
					if (!IsGraphBipartite(startColor.Swap(), incindentNodes))
						return false;
					queue.Enqueue(incindentNodes);
				}
				//startNode++;
			}
			return true;
		}

		
	}
}