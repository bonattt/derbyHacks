using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    class GuardCommunication
    {
        private static GuardCommunication Instance;

        private Dictionary<Point, int> pointsInvestigated;
        private HashSet<Point> pointsUnderInvestigation;

        private GuardCommunication()
        {
            pointsUnderInvestigation = new HashSet<Point>();
            pointsInvestigated = new Dictionary<Point, int>();
        }

        public static GuardCommunication GetInstance()
        {
            if (Instance == null) {
                Instance = new GuardCommunication();
            }
            return Instance;
        }

        public void PassTurn()
        {
            foreach (Point p in pointsInvestigated.Keys)
            {
                pointsInvestigated[p] -= 1;
            }
        }

        public Point getNewInvestigationPoint(EnemyUnit e)
        {
           // Dictionary<Point, double> dict = GetRandomPoints();
           // RankDictionary(dict, TacticsGrid.GetInstance().GetEntityPosition(e));
           // return SelectBest(dict);
            return GetCompletelyRandomPoint();
        }

        private Point GetCompletelyRandomPoint()
        {
            int xUpper = TacticsGrid.ConvertIntToGrid(TacticsGrid.GetInstance().Width);
            int yUpper = TacticsGrid.ConvertIntToGrid(TacticsGrid.GetInstance().Height);
            int xLower = 1;
            int yLower = 1;
            Random r = new Random();
            

                int xRand = (int)Math.Round((r.NextDouble() * (xUpper - xLower)) + xLower, 0);
                int yRand = (int)Math.Round((r.NextDouble() * (yUpper - yLower)) + yLower, 0);
                return new Point(xRand, yRand);
             
        }

        public Dictionary<Point, double> GetRandomPoints()
        {
            Dictionary<Point, double> dict = new Dictionary<Point, double>();
            int xUpper = TacticsGrid.ConvertIntToGrid(TacticsGrid.GetInstance().Width);
            int yUpper = TacticsGrid.ConvertIntToGrid(TacticsGrid.GetInstance().Height);
            int xLower = 1;
            int yLower = 1;
            Random r = new Random();
            for (int k = 0; k < 1; k++)
            {
                int xRand = (int)Math.Round((r.NextDouble() * (xUpper - xLower)) + xLower, 0);
                int yRand = (int)Math.Round((r.NextDouble() * (yUpper - yLower)) + yLower, 0);
                Point p = new Point(xRand, yRand);
                dict.Add(p, 0);

            }
            return dict;
        }

        private void RankDictionary(Dictionary<Point, double> dict, Point guard)
        {
            foreach (Point p in dict.Keys)
            {
                //RankByOtherPoints(dict, p);
                //RankByProximity(dict, p, guard);
            }
        }

        private void RankByProximity(Dictionary<Point, double> dict, Point intP, Point guardPt)
        {
            double distance = AStar.DistanceBetween(intP, guardPt);
            dict[intP] += 2 * Math.Pow(distance, .33);
        }
        private void RankByOtherPoints(Dictionary<Point, double> dict, Point p1)
        {
            /*Point nearest = new Point(Int32.MinValue, Int32.MinValue);
            double bestDist = Double.MaxValue;
            foreach (Point p2 in pointsInvestigated.Keys)
            {
                double dist = AStar.DistanceBetween(p1, p2);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    nearest = p1;
                }
            }
            double mod = 7 - dict[nearest];
            double bonus = 1; // figure this out.
            dict[p1] += mod * bonus;*/
        }

        private Point SelectBest(Dictionary<Point, double> dict)
        {
            Point best = new Point(Int32.MaxValue, Int32.MaxValue);
            double rank = Int32.MinValue;
            foreach (Point p in dict.Keys)
            {
                if (dict[p] > rank)
                {
                    rank = dict[p];
                    best = p;
                }
            }
            return best;
        }
    }
}
