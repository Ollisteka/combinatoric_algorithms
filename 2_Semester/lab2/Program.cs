using System.IO;

namespace lab2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            args = File.ReadAllLines("in.txt");
            var finder = new MaximumMatchingFinder(args);
        }
    }
}