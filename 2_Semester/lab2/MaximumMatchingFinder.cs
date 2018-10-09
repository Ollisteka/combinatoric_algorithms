using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public class MaximumMatchingFinder
    {
        private readonly EdgeType[] choice; //отмечаем, через какую дугу попали в v - прямую или обратную
        private readonly int[] previous; //вершина, из которой пометили текущую
        private readonly double[] minimalFlow; //если m[v]!=Infinity, то существует ненасыщенная (s, v)-цепь P, и h(P)=m[v].

        public readonly int MaxX;
        public readonly int MaxY;
        public readonly int Sink;
        public readonly int Source = 0;
        public readonly int[,] Throughput;
        public double[,] Flows;
        public readonly List<int> Vertices;
        public double MaxFlow;

        public Func<int, int> GetFlatVertexForY => v => v + MaxX;

        public MaximumMatchingFinder(string[] args)
        {
            var setsSizes = args[0].ReadInts();
            MaxX = setsSizes[0];
            MaxY = setsSizes[1];
            Sink = MaxX + MaxY + 1;
            Vertices = Enumerable.Range(0, Sink + 1).ToList();

            var dim = Vertices.Count;
            Throughput = new int[dim, dim];
            Flows = new double[dim, dim];
            previous = new int[dim];
            minimalFlow = new double[dim];
            choice = new EdgeType[dim];
            
            InitGraph(args);
            InitSource();
            InitSink();
        }

        private void InitGraph(string[] args)
        {
            for (var i = 0; i < MaxX; i++)
            {
                var x = i + 1;
                var line = args[x];

                var yCoordinates = line.ReadInts();
                for (var y = 0; y < MaxY; y++)
                    Throughput[x, GetFlatVertexForY(y + 1)] =
                        Throughput[GetFlatVertexForY(y + 1), x] = yCoordinates[y];
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
                FindAugmentingPath();
                if (double.IsPositiveInfinity(minimalFlow[Sink]))
                    continue;

                MaxFlow += minimalFlow[Sink];
                var v = Sink;
                while (v != Source)
                {
                    var w = previous[v];
                    if (choice[v] == EdgeType.Straight)
                        Flows[w, v] = Flows[w, v] + minimalFlow[Sink];
                    else
                        Flows[v, w] = Flows[v, w] - minimalFlow[Sink];
                    v = w;
                }
            } while (!double.IsPositiveInfinity(minimalFlow[Sink]));
        }

        /// <summary>
        ///     Если в конце labels[sink] не бесконечность - то это существует f-дополняющая цепь P (восстанавливаем по previous).
        ///     При этом h(P) = labels[sink] =>  текущий поток можно увеличить на эту величину.
        /// </summary>
        private void FindAugmentingPath()
        {
            for (var i = 0; i < minimalFlow.Length; i++)
                minimalFlow[i] = double.PositiveInfinity;

            var queue = new Queue<int>();
            queue.Enqueue(Source);
            previous[Source] = -1;
            while (double.IsPositiveInfinity(minimalFlow[Sink]) && queue.Count != 0)
            {
                var w = queue.Dequeue();
                //идём по прямым рёбрам
                foreach (var vertex in Vertices)
                    if (double.IsPositiveInfinity(minimalFlow[vertex]) && Throughput[w, vertex] > Flows[w, vertex])
                    {
                        minimalFlow[vertex] = Math.Min(minimalFlow[w], Throughput[w, vertex] - Flows[w, vertex]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = EdgeType.Straight;
                    }

                //идём по обратным
                foreach (var vertex in Vertices.Skip(1))
                    if (double.IsPositiveInfinity(minimalFlow[vertex]) && Flows[vertex, w] > 0)
                    {
                        minimalFlow[vertex] = Math.Min(minimalFlow[w], Throughput[vertex, w] - Flows[vertex, w]);
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
                    if (Math.Abs(Flows[x, GetFlatVertexForY(y)]) < 0.0000001)
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