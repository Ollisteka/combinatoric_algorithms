using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _1_task
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			args = File.ReadAllLines("in.txt");
			var reader = new BipartiteGraphReader(args);

			var success = reader.IsGraphBipartite();
			if (!success)
				File.WriteAllText("out.txt", "N");
			else
				WriteParts(reader);
		}

		private static void WriteParts(BipartiteGraphReader reader)
		{
			var firstRow = string.Join(" ", reader.PartEven.Select(x => x.ToString()));
			var secondRow = string.Join(" ", reader.PartOdd.Select(x => x.ToString()));
			var result = new List<string> {"Y", firstRow, secondRow};
			File.WriteAllLines("out.txt", result);
		}
	}
}