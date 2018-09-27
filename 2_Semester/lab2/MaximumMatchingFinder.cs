using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class MaximumMatchingFinder
    {
        private readonly int[] choice; //отмечаем, через какую дугу попали в v - прямую или обратную
        private readonly int[,] throughput;
        private readonly double[] labels;
        private readonly int MaxX;
        private readonly int MaxY;
        private const int Source = 0;
        private readonly int Sink;
        private readonly int[] previous; //вершина, из которой пометили текущую
        private int[,] flows;
        private Func<int, int> GetFlatVerticeForY => v => v + MaxX;
        private Func<int, int> GetOriginVerticeForY => v => v - MaxX;
        private readonly List<int> Vertices;


        public MaximumMatchingFinder(string[] args)
        {
            var setsSizes = args[0].ReadInts();
            MaxX = setsSizes[0];
            MaxY = setsSizes[1];
            Sink = MaxX + MaxY + 1;
            throughput = new int[MaxX + 2, MaxY + 2];
            flows = new int[MaxX + 2, MaxY + 2];
            previous = new int[MaxX + MaxY + 2];
            labels = new double[MaxX + MaxY + 2];
            Vertices = Enumerable.Range(0, Sink + 1).ToList();
            InitGraphMatrix(args);
            InitSource();
            InitSink();
        }

        private void InitGraphMatrix(string[] args)
        {
            //todo здесь уже надо использовать GetFlatVerticeForY!!!
            for (var i = 0; i < MaxX; i++)
            {
                var x = i + 1;
                var line = args[x];
                for (var j = 0; j < MaxY; j++)
                {
                    var yCoordinates = line.ReadInts();
                    for (var y = 0; y < MaxY; y++)
                        throughput[x, y + 1] = yCoordinates[y];
                }
            }
        }

        private void InitSource()
        {
            for (var x = 1; x <= MaxX; x++)
                throughput[x, 0] = 1;
            for (var y = 1; y <= MaxY; y++)
                throughput[0, y] = 1;
        }

        private void InitSink()
        {
            for (var x = MaxX; x >= 1; x--)
                throughput[x, MaxY + 1] = 1;
            for (var y = MaxY; y >= 1; y--)
                throughput[MaxX + 1, y] = 1;
        }

        private void Label()
        {
            for (var i = 0; i < labels.Length; i++)
                labels[i] = double.PositiveInfinity;
            var queue = new Queue<int>();
            queue.Enqueue(Source);
            previous[Source] = -1;
            while (double.IsPositiveInfinity(labels[Sink]) && queue.Count != 0)
            {
                var w = queue.Dequeue();
                foreach (var vertex in Vertices)
                {
                    if (double.IsPositiveInfinity(labels[vertex]) && throughput[w, vertex] > flows[w, vertex])
                    {
                        labels[vertex] = Math.Min(labels[w], throughput[w, vertex] - flows[w, vertex]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = 1;
                    }
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