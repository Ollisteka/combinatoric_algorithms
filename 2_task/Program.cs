using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_task
{
	public class Program
	{
		
		static void Main(string[] args)
		{
			var finder = new PathFinder(args);
			if (!finder.CanFindWay())
				File.WriteAllText("out.txt", "N");
			else
			{
				WriteAnswer(finder);
			}
		}

		private static void WriteAnswer(PathFinder finder)
		{
			var result = new List<string> {"Y"};
			result.AddRange(finder.way.Select(point => point.ToString()));
			File.WriteAllLines("out.txt", result);
		}
	}
}
