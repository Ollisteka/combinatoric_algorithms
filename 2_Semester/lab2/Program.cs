using System.IO;

namespace lab2
{
    internal class Program
    {
        private static void Main()
        {
            var args = File.ReadAllLines("in.txt");
            var finder = new MaximumMatchingFinder(args);
            finder.FindMaxFlow();
            File.WriteAllLines("out.txt", finder.GetMatching());
        }
    }
}