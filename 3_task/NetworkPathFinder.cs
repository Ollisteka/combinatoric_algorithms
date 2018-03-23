using System.Collections.Generic;
using System.Linq;

namespace _3_task
{
    public class NetworkPathFinder
    {
        public readonly int Start;
        public readonly int Finish;
        public readonly int NodeCount;
        private List<int> allNodes;
        private List<int> allNodesWithoutStart;

        private Dictionary<int,int> DistanceTo = new Dictionary<int, int>();
        private Dictionary<int,int> Previous = new Dictionary<int, int>();
        private int[,] Weights;
        public NetworkPathFinder(string[] args)
        {
            NodeCount = int.Parse(args[0]);
            Start = int.Parse(args[NodeCount + 1]);
            Finish = int.Parse(args[NodeCount + 2]);
            allNodes = Enumerable.Range(1, NodeCount).ToList();
            allNodesWithoutStart = allNodes.Where(x => x != Start).ToList();
            Weights = new int[NodeCount + 1, NodeCount + 1];
            
            for (int i = 1; i <= NodeCount; i++)
            {
                for (int j = 1; j <= NodeCount; j++)
                {
                    if (i == j)
                        Weights[i, j] = 0;
                    else
                        Weights[i, j] = 999999;
                }
            }
            for (int node = 1; node <= NodeCount; node++)
            {
                var line = args[node].Split(' ').Select(int.Parse).ToList();
                line = line.Take(line.Count - 1).ToList();
                for (int j = 0; j < line.Count; j += 2)
                {
                    var previousNode = line[j];
                    var weight = line[j + 1];
                    Weights[previousNode, node] = weight;
                }
            }
            InitDistances(NodeCount + 1);

        }

        private void InitDistances(int size)
        {
            DistanceTo.Add(Start, 0);
            Previous.Add(Start, 0);
            foreach (var node in allNodesWithoutStart)
            {
                DistanceTo.Add(node, Weights[Start, node]);
                Previous.Add(node, Start);
            }
        }

        private void CalculateDistances()
        {
            for (int k = 1; k < NodeCount-2; k++)
            {
                foreach (var v in allNodesWithoutStart)
                {
                    foreach (var w in allNodes)
                    {
                        var oldDistance = DistanceTo[v];
                        var newDistance = DistanceTo[w] + Weights[w, v];
                        if (DistanceTo[w] + Weights[w, v] < DistanceTo[v])
                        {
                            DistanceTo[v] = DistanceTo[w] + Weights[w, v];
                            Previous[v] = w;
                        }
                    }
                }
            }
        }

        public Stack<int> GetWay()
        {
            CalculateDistances();
            if (DistanceTo[Finish] == int.MaxValue)
                return null;
            var result = new Stack<int>();
            result.Push(Finish);
            var currentNode = Finish;
            while (Previous[currentNode] != 0)
            {
                currentNode = Previous[currentNode];
                result.Push(currentNode);
            }

            return result;
        }
    }
}