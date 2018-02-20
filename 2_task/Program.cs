using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_task
{
	enum State
	{
		Empty,
		Wall,
		Visited
	}

	class Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Point m))
				return false;

			return m.X == this.X && m.Y == this.Y;
		}
	}

	class Program
	{
		private static Dictionary<Point, Point> prev = new Dictionary<Point, Point>();
		private static Point Start;
		private static Point Finish;
		private static List<Point> Way = new List<Point>();
		static void Main(string[] args)
		{
		}
		public static bool Visit(State[,] map, int x, int y, Point prevPoint)
		{
			if (x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1)) return false;
			if (map[x, y] != State.Empty) return false;
			map[x, y] = State.Visited;
			var currentPoint = new Point(x, y);
			prev.Add(currentPoint, prevPoint);
			if (currentPoint.Equals(Finish))
			{
				FillWay();
				return true;
			}
			for (var dy = -1; dy <= 1; dy++)
			for (var dx = -1; dx <= 1; dx++)
				if (dx != 0 && dy != 0) continue;
				else
				{
					if (Visit(map, x + dx, y + dy, new Point(x, y)))
						return true;
				}
			return false;
		}

		private static void PrintAnswer()
		{
			Way.Reverse();
			Console.WriteLine("Y");
			foreach (var point in Way)
			{
				Console.WriteLine($"{point.X} {point.Y}");
			}
		}

		private static void FillWay()
		{
			var currentPoint = Finish;
			Way.Add(currentPoint);
			while (!currentPoint.Equals(Start))
			{
				Way.Add(prev[currentPoint]);
				currentPoint = prev[currentPoint];
			}
			Way.Add(currentPoint);
		}
	}
}
