using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab1
{
    public class MSTFinder
    {
        public readonly List<int> Vertices;
        public readonly List<Edge> Edges = new List<Edge>();
        private readonly int[] forest;
        private readonly int[] forestSize;
        private readonly int[] next;

        public MSTFinder(string[] args)
        {
            var n = int.Parse(args[0]);
            Vertices = Enumerable.Range(1, n).ToList();
            forest = new int[Vertices.Count + 1];
            forestSize = new int[Vertices.Count + 1];
            next = new int[Vertices.Count + 1];
            for (int i = 0; i < n; i++)
            {
                var v = i + 1;
                var line = args[v].Split(' ').Where(e => e != "").Select(int.Parse).ToList();
                for (int j = 0; j < line.Count - 1; j += 2)
                {
                    var adjacentNode = line[j];
                    var weight = line[j + 1];
                    if (!Edges.Any(edge => edge.AdjacentNodes.Contains(v) && edge.AdjacentNodes.Contains(adjacentNode)))
                        Edges.Add(new Edge(v, adjacentNode, weight));
                }
            }
        }

        public List<Edge> FindMinimalSpanningTree()
        {
            var mst = new List<Edge>();
            var queue = GetSortedEdgesQueue();

            foreach (var v in Vertices)
            {
                forest[v] = next[v] = v;
                forestSize[v] = 1;
            }

            while (mst.Count != Vertices.Count - 1)
            {
                var smallestEdge = queue.Dequeue();
                var w = smallestEdge.AdjacentNodes[0];
                var v = smallestEdge.AdjacentNodes[1];
                var w_tree = forest[w];
                var v_tree = forest[v];
                if (w_tree == v_tree)
                    continue;
                if (forestSize[w_tree] > forestSize[v_tree])
                    Merge(v, w, v_tree, w_tree);
                else Merge(w, v, w_tree, v_tree);
                mst.Add(smallestEdge);
            }

            return mst;
        }

        private void Merge(int vertexFromSmall, int vertexFromBig, int smallerTree, int biggerTree)
        {
            forest[vertexFromSmall] = biggerTree;
            var u = next[vertexFromSmall];
            while (forest[u] != biggerTree)
            {
                forest[u] = biggerTree;
                u = next[u];
            }

            forestSize[biggerTree] += forestSize[smallerTree];
            var x = next[vertexFromSmall];
            var y = next[vertexFromBig];
            next[vertexFromSmall] = y;
            next[vertexFromBig] = x;
        }

        public Queue<Edge> GetSortedEdgesQueue()
        {
            var queue = new Queue<Edge>();
            foreach (var edge in Edges.OrderBy(e => e.Weight))
                queue.Enqueue(edge);
            return queue;
        }

        public string[] PrintMST(List<Edge> mst)
        {
            var result = new List<string>();
            foreach (var vertex in Vertices)
            {
                var line = new StringBuilder();
                var adjacentNodes = mst.Where(edge => edge.AdjacentNodes.Contains(vertex))
                    .Select(edge =>
                    {
                        var adjNode = edge.AdjacentNodes.First(v => v != vertex);
                        var weight = edge.Weight;
                        return (adjNode: adjNode, weight: weight);
                    }).OrderBy(el => el.adjNode);
                foreach (var (adjNode, weight) in adjacentNodes)
                    line.Append($"{adjNode} {weight} ");
                result.Add($"{line}0");
            }

            result.Add(mst.Sum(edge => edge.Weight).ToString());
            return result.ToArray();
        }
    }
}