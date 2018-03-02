using System.Collections.Generic;
using System.Linq;

namespace _2_task
{
	public class PathFinder
	{
		public readonly Point Finish;
		private readonly State[,] map;
		public readonly Point Start;

		private readonly List<Point> Directions = new List<Point>
		{
			new Point(-1, 0),
			new Point(1, 0),
			new Point(0, -1),
			new Point(0, 1)
		};


		public Dictionary<Point, Point> Father = new Dictionary<Point, Point>();
		public List<Point> way = new List<Point>();

		public PathFinder(string[] args)
		{
			var maxX = int.Parse(args[0]);
			var maxY = int.Parse(args[1]);
			map = new State[maxX + 1, maxY + 1];
			for (var x = 2; x < 2 + maxX; x++)
			{
				var line = args[x].Split(' ');
				var xCoord = x - 1;
				for (var y = 0; y < line.Length; y++)
				{
					var yCoord = y + 1;
					if (line[y] == "1")
						map[xCoord, yCoord] = State.Wall;
					if (line[y] == "0")
						map[xCoord, yCoord] = State.Empty;
				}
			}
			var start = args[2 + maxX].Split().Select(int.Parse).ToList();
			var finish = args[2 + maxX + 1].Split().Select(int.Parse).ToList();
			Start = new Point(start[0], start[1]);
			Finish = new Point(finish[0], finish[1]);
		}

		public bool CanFindWay()
		{
			DFS(Start);
			if (!Father.ContainsKey(Finish))
				return false;

			way = FillWay();
			return true;
		}

		private List<Point> FillWay()
		{
			var result = new List<Point>();
			var currentPoint = Finish;
			while (!currentPoint.Equals(Start))
			{
				result.Add(currentPoint);
				currentPoint = Father[currentPoint];
			}
			result.Add(Start);
			result.Reverse();
			return result.ToList();
		}

		private void DFS(Point startNode)
		{
			var stack = new Stack<Point>();
			stack.Push(startNode);
			while (stack.Count != 0)
			{
				var point = stack.Pop();
				if (point.X < 0 || point.X >= map.GetLength(0) || point.Y < 0 || point.Y >= map.GetLength(1)) continue;
				if (map[point.X, point.Y] != State.Empty) continue;
				map[point.X, point.Y] = State.Visited;

				foreach (var shift in Directions)
				{
					var incindentPoint = point + shift;
					if (map[incindentPoint.X, incindentPoint.Y] == State.Wall || map[incindentPoint.X, incindentPoint.Y] == State.Visited)
						continue;
					stack.Push(incindentPoint);
					Father[incindentPoint] = point;
				}
			}
		}
	}
}