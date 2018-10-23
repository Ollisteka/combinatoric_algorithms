using System.Collections.Generic;
using System.Linq;
using System.Text;
using lab2;

namespace lab3
{
    public class RalphHelper
    {
        public readonly List<Point> MeetingPoints;
        public readonly List<Point> InterestingPlaces;
        public Dictionary<Point, int> MeetMap = new Dictionary<Point, int>();
        public Dictionary<Point, int> InterestingPlacesMap = new Dictionary<Point, int>();
        public int[,] AdjacentGraph;

        public RalphHelper(string[] args)
        {
            MeetingPoints = args[1].ReadPoints();
            InterestingPlaces = args[2].ReadPoints();
            for (var i = 0; i < MeetingPoints.Count; i++)
                MeetMap[MeetingPoints[i]] = i;

            for (var i = 0; i < InterestingPlaces.Count; i++)
                InterestingPlacesMap[InterestingPlaces[i]] = i;


            AdjacentGraph = new int[MeetingPoints.Count, InterestingPlaces.Count];
            FillReachablePlaces();
        }

        public void FillReachablePlaces()
        {
            for (var i = 0; i < MeetingPoints.Count - 1; i++)
                foreach (var reachablePlace in GetReachablePlaces(MeetingPoints[i], MeetingPoints[i + 1]))
                    AdjacentGraph[i, InterestingPlacesMap[reachablePlace]] = 1;
        }

        private IEnumerable<Point> GetReachablePlaces(Point from, Point to)
        {
            var bobDistance = from.DistanceTo(to);
            foreach (var interestingPlace in InterestingPlaces)
            {
                var ralphDistance = from.DistanceTo(interestingPlace) + interestingPlace.DistanceTo(to);
                if (ralphDistance <= 2 * bobDistance)
                    yield return  interestingPlace;
            }
        }

        public int HelpRalph()
        {
            var maxMatchingFinder = new MaximumMatchingFinder(GetInputForMatching());
            maxMatchingFinder.FindMaxFlow();
            var maxMatching = maxMatchingFinder.GetMatching();
            return MeetingPoints.Count + maxMatching.Count(x => x != "0");
        }

        private string[] GetInputForMatching()
        {
            var maxX = AdjacentGraph.GetLength(0);
            var maxY = AdjacentGraph.GetLength(1);
            var result = new List<string> {$"{maxX} {maxY}"};
            for (var x = 0; x < maxX; x++)
            {
                var line = new StringBuilder();
                for (var y = 0; y < maxY; y++)
                    line.Append($"{AdjacentGraph[x, y]} ");
                result.Add(line.ToString().Trim());
            }

            return result.ToArray();
        }
    }
}