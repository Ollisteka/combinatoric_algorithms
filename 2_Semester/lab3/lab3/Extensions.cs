using System.Collections.Generic;
using lab2;

namespace lab3
{
    public static class Extensions
    {
        public static List<Point> ReadPoints(this string line)
        {
            var result = new List<Point>();
            var coords = line.ReadInts();
            for (var i = 0; i < coords.Length; i+=2)
                result.Add(new Point(coords[i], coords[i+1]));
            return result;
        }
    }
}