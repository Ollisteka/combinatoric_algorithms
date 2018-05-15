using System;
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

        private readonly Dictionary<int, double> distanceTo = new Dictionary<int, double>();
        private readonly Dictionary<int,int> previous = new Dictionary<int, int>();
        private double[,] weights;


        public NetworkPathFinder(string[] args)
        {
            Cost = 1;
            NodeCount = int.Parse(args[0]);
            Start = int.Parse(args[NodeCount + 1]);
            Finish = int.Parse(args[NodeCount + 2]);
            allNodes = Enumerable.Range(1, NodeCount).ToList();
            allNodesWithoutStart = allNodes.Where(x => x != Start).ToList();
            InitWeights();
            for (int node = 1; node <= NodeCount; node++)
            {
                var line = args[node].Split(' ').Where(e => e != "").Select(int.Parse).ToList();
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
            weights = new Double[NodeCount + 1, NodeCount + 1];

            for (int i = 1; i <= NodeCount; i++)
            {
                for (int j = 1; j <= NodeCount; j++)
                {
                    if (i == j)
                        weights[i, j] = 0;
                    else
                        weights[i, j] = double.NegativeInfinity;
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
            for (var edge = 1; edge < NodeCount-2; edge++)
            {
                foreach (var to in allNodesWithoutStart)
                {
                    foreach (var from in allNodes)
                    {
                        var dist = distanceTo[from];
                        var weight = weights[from, to];
                        var multDist = 0.0;
                        if (dist == 0 && weight > 0)
                            multDist = weight;
                        else if (dist > 0 && weight == 0)
                            multDist = dist;
                        else if (dist > 0 && weight > 0)
                            multDist = dist * weight;
                        else multDist = double.NegativeInfinity;

                        var newDist = distanceTo[from] + weights[from, to];
                        var oldDist = distanceTo[to];
                        if (multDist > distanceTo[to])
                        {
                            distanceTo[to] = multDist;
                            previous[to] = from;
                        }
                    }
                }
            }
        }

        public Stack<int> GetWay()
        {
            CalculateDistances();
            if (double.IsNegativeInfinity(distanceTo[Finish]))
                return null;
            var result = new Stack<int>();
            result.Push(Finish);
            var currentNode = Finish;
            while (previous[currentNode] != 0)
            {
            //    Cost *= (int)Math.Abs(weights[previous[currentNode], currentNode]);
                currentNode = previous[currentNode];
                result.Push(currentNode);
            }

            Cost = (int)distanceTo[Finish];
            return result;
        }
        
    }
}