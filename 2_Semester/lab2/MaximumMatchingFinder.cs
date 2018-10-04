using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class MaximumMatchingFinder
    {
        private const int offset = 2;
        private readonly EdgeType[] choice; //отмечаем, через какую дугу попали в v - прямую или обратную
        private readonly double[] labels;
        public readonly int MaxX;
        public readonly int MaxY;
        private readonly int[] previous; //вершина, из которой пометили текущую
        public readonly int Sink;
        public readonly int Source = 0;
        public readonly int[,] Throughput;
        public readonly List<int> Vertices;
        public double[,] Flows;
        public double MaxFlow;

        public MaximumMatchingFinder(string[] args)
        {
            var setsSizes = args[0].ReadInts();
            MaxX = setsSizes[0];
            MaxY = setsSizes[1];
            Sink = MaxX + MaxY + 1;
            Throughput = new int[MaxX + MaxY + offset, MaxX + MaxY + offset];
            Flows = new double[MaxX + MaxY + offset, MaxX + MaxY + offset];
            previous = new int[MaxX + MaxY + offset];
            labels = new double[MaxX + MaxY + offset];
            choice = new EdgeType[MaxX + MaxY + offset];
            Vertices = Enumerable.Range(0, Sink + 1).ToList();
            InitGraphMatrix(args);
            InitSource();
            InitSink();
        }

        public Func<int, int> GetFlatVerticeForY => v => v + MaxX;

        private void InitGraphMatrix(string[] args)
        {
            for (var i = 0; i < MaxX; i++)
            {
                var x = i + 1;
                var line = args[x];

                var yCoordinates = line.ReadInts();
                for (var y = 0; y < MaxY; y++)
                    Throughput[x, GetFlatVerticeForY(y + 1)] =
                        Throughput[GetFlatVerticeForY(y + 1), x] = yCoordinates[y];
            }
        }

        private void InitSource()
        {
            for (var v = 1; v <= MaxX; v++)
                Throughput[Source, v] = 1;
        }

        private void InitSink()
        {
            for (var v = MaxX + 1; v < Sink; v++)
                Throughput[v, Sink] = 1;
        }

        public void FindMaxFlow()
        {
            foreach (var v in Vertices)
            foreach (var w in Vertices)
                Flows[v, w] = 0;
            do
            {
                Label();
                if (double.IsPositiveInfinity(labels[Sink]))
                    continue;

                MaxFlow += labels[Sink];
                var v = Sink;
                while (v != Source)
                {
                    var w = previous[v];
                    if (choice[v] == EdgeType.Straight)
                        Flows[w, v] = Flows[w, v] + labels[Sink];
                    else
                        Flows[v, w] = Flows[v, w] - labels[Sink];
                    v = w;
                }
            } while (!double.IsPositiveInfinity(labels[Sink]));
        }

        /// <summary>
        ///     Если в конце labels[sink] не бесконечность - то это существует f-дополняющая цепь P (восстанавливаем по previous).
        ///     При этом h(P) = labels[sink] =>  текущий поток можно увеличить на эту величину.
        /// </summary>
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
                //идём по прямым рёбрам
                foreach (var vertex in Vertices)
                    if (double.IsPositiveInfinity(labels[vertex]) && Throughput[w, vertex] > Flows[w, vertex])
                    {
                        labels[vertex] = Math.Min(labels[w], Throughput[w, vertex] - Flows[w, vertex]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = EdgeType.Straight;
                    }

                //идём по обратным
                foreach (var vertex in Vertices.Skip(1))
                    if (double.IsPositiveInfinity(labels[vertex]) && Flows[vertex, w] > 0)
                    {
                        labels[vertex] = Math.Min(labels[w], Throughput[vertex, w] - Flows[vertex, w]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = EdgeType.Reverse;
                    }
            }
        }

        public string[] GetMatching()
        {
            var result = new List<string>();
            for (var x = 1; x <= MaxX; x++)
            {
                var found = false;
                for (var y = 1; y <= MaxY; y++)
                {
                    if (Math.Abs(Flows[x, GetFlatVerticeForY(y)]) < 0.0000001)
                        continue;

                    result.Add(y.ToString());
                    found = true;
                    break;
                }

                if (!found)
                    result.Add("0");
            }

            return result.ToArray();
        }
    }
}