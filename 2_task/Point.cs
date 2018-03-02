namespace _2_task
{
	public struct Point
	{
		public int X;
		public int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"{X} {Y}";
		}
		public static Point operator +(Point one, Point two)
		{
			return new Point(one.X + two.X, one.Y + two.Y);
		}
	}
}