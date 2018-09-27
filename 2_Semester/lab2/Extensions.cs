using System.Linq;

namespace lab2
{
    public static class Extensions
    {
        public static int[] ReadInts(this string line)
        {
            return line.Split().Select(int.Parse).ToArray();
        }
    }
}