using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public static class Extensions
    {
        public static int[] ReadInts(this string line)
        {
            return line.Split().Select(int.Parse).ToArray();
        }
    }

    public class MaximumMatchingFinder
    {
        private int[,] flows;
        private readonly int[,] graph;
        private readonly double[] labels;
        private readonly int[] previous; //вершина, из которй пометили текущую
        private readonly int[] choice; //отмечаем, через какую дугу попали в v - прямую или обратную
        private Func<int, int> GetFlatVerticeForY => x => x + graph.GetLength(0);

        public MaximumMatchingFinder(string[] args)
        {
            var setsSizes = args[0].ReadInts();
            var k = setsSizes[0];
            var l = setsSizes[1];
            graph = new int[k + 1, l + 1];
            flows = new int[k + 1, l + 1];
            previous = new int[k + l + 2];
            labels = new double[k + l + 2];
            for (var i = 0; i < labels.Length; i++)
                labels[i] = double.PositiveInfinity;

            for (var i = 0; i < k; i++)
            {
                var x = i + 1;
                var line = args[x];
                for (var j = 0; j < l; j++)
                {
                    var yCoordinates = line.ReadInts();
                    for (var y = 0; y < l; y++)
                        graph[x, y + 1] = yCoordinates[y];
                }
            }
        }

        private void DFS(int startNode)
        {
            var stack = new Stack<int>();
            var visited = new HashSet<int>();
            stack.Push(startNode);
            while (stack.Count != 0)
            {
                var point = stack.Pop();
                if (visited.Contains(point)) continue;
                visited.Add(point);

                foreach (var shift in Directions)
                {
                    var incindentPoint = point + shift;
                    if (map[incindentPoint.X, incindentPoint.Y] == State.Wall ||
                        map[incindentPoint.X, incindentPoint.Y] == State.Visited)
                        continue;
                    stack.Push(incindentPoint);
                    Father[incindentPoint] = point;
                }
            }
        }
    }
}