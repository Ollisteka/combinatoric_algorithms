using System.Collections.Generic;
using System.Linq;

namespace _3_task
{
    public class NetworkPathFinder
    {
        public readonly int Start;
        public readonly int Finish;
        public readonly int NodeCount;

        public int Cost { get; private set; }

        private readonly List<int> allNodes;
        private readonly List<int> allNodesWithoutStart;

        private readonly Dictionary<int,int> distanceTo = new Dictionary<int, int>();
        private readonly Dictionary<int,int> previous = new Dictionary<int, int>();
        private int[,] weights;

        private const int Infinity = 9999999;

        public NetworkPathFinder(string[] args)
        {
            NodeCount = int.Parse(args[0]);
            Start = int.Parse(args[NodeCount + 1]);
            Finish = int.Parse(args[NodeCount + 2]);
            allNodes = Enumerable.Range(1, NodeCount).ToList();
            allNodesWithoutStart = allNodes.Where(x => x != Start).ToList();
            InitWeights();
            for (int node = 1; node <= NodeCount; node++)
            {
                var line = args[node].Split(' ').Select(int.Parse).ToList();
                line = line.Take(line.Count - 1).ToList();
                for (int j = 0; j < line.Count; j += 2)
                {
                    var previousNode = line[j];
                    var weight = line[j + 1];
                    weights[previousNode, node] = weight;
                }
            }
            InitDistances();
        }

        private void InitWeights()
        {
            weights = new int[NodeCount + 1, NodeCount + 1];

            for (int i = 1; i <= NodeCount; i++)
            {
                for (int j = 1; j <= NodeCount; j++)
                {
                    if (i == j)
                        weights[i, j] = 0;
                    else
                        weights[i, j] = Infinity;
                }
            }
        }

        private void InitDistances()
        {
            distanceTo.Add(Start, 0);
            previous.Add(Start, 0);
            foreach (var node in allNodesWithoutStart)
            {
                distanceTo.Add(node, weights[Start, node]);
                previous.Add(node, Start);
            }
        }

        private void CalculateDistances()
        {
            for (int edge = 1; edge < NodeCount-2; edge++)
            {
                foreach (var v in allNodesWithoutStart)
                {
                    foreach (var w in allNodes)
                    {
                        if (distanceTo[w] + weights[w, v] < distanceTo[v])
                        {
                            distanceTo[v] = distanceTo[w] + weights[w, v];
                            previous[v] = w;
                        }
                    }
                }
            }
        }

        public Stack<int> GetWay()
        {
            CalculateDistances();
            if (distanceTo[Finish] == Infinity)
                return null;
            var result = new Stack<int>();
            result.Push(Finish);
            var currentNode = Finish;
            while (previous[currentNode] != 0)
            {
                currentNode = previous[currentNode];
                result.Push(currentNode);
            }

            Cost = distanceTo[Finish];
            return result;
        }
        
    }
}