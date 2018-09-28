using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    public enum EdgeType
    {
        Reverse = -1,
        None = 0,
        Straight = 1,
    }
    public class MaximumMatchingFinder
    {
        private readonly EdgeType[] choice; //отмечаем, через какую дугу попали в v - прямую или обратную
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

                var yCoordinates = line.ReadInts();
                for (var y = 0; y < MaxY; y++)
                    throughput[x, GetFlatVerticeForY(y + 1)] = throughput[GetFlatVerticeForY(y + 1), x] = yCoordinates[y];
                
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
        
        /// <summary>
        /// Если в конце labels[sink] не бесконечность - то это существует f-дополняющая цепь P (восстанавливаем по previous).
        /// При этом h(P) = labels[sink] =>  текущий поток можно увеличить на эту величину.
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
                {
                    if (double.IsPositiveInfinity(labels[vertex]) && throughput[w, vertex] > flows[w, vertex])
                    {
                        labels[vertex] = Math.Min(labels[w], throughput[w, vertex] - flows[w, vertex]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = EdgeType.Straight;
                    }
                }
                //идём по обратным
                foreach (var vertex in Vertices.Skip(1))
                {
                    if (double.IsPositiveInfinity(labels[vertex]) && flows[vertex, w] > 0)
                    {
                        labels[vertex] = Math.Min(labels[w], throughput[vertex, w] - flows[vertex, w]);
                        previous[vertex] = w;
                        queue.Enqueue(vertex);
                        choice[vertex] = EdgeType.Reverse;
                    }
                }

            }
        }

        
    }
}