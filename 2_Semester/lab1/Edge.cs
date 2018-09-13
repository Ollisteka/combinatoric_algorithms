using System.Collections.Generic;

namespace lab1
{
    public class Edge
    {
        public readonly List<int> AdjacentNodes;
        public readonly int Weight;

        public Edge(int vertex1, int vertex2, int weight)
        {
            AdjacentNodes = new List<int> {vertex1, vertex2};
            Weight = weight;
        }
    }
}