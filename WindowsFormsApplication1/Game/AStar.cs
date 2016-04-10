using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public class AStar
    {

        private Node current, start;            
        private Point end;
        private HashSet<Node> openSet, closedSet;

        public AStar(Point startPt, Point endPt)
        {
            openSet = new HashSet<Node>();
            closedSet = new HashSet<Node>();
            end = endPt;
            start = new StartNode(startPt, endPt);
        }

        public bool ClearLineOfSight()
        {
            Point current = start.pos;
            double currentDist = DistanceBetween(current, end);
            try
            {
                while (!current.Equals(end))
                {
                    current = getNextLoS(current);
                }
            }
            catch (UnreachableDestinationException)
            {
                return false;
            }
            return true;
        }

        private Point getNextLoS(Point current)
        {
            Point[] adjPts = getAdjacentPoints(current);
            Point best = adjPts[0];
            double bestDist = DistanceBetween(best, end);
            for (int i = 1; i < adjPts.Length; i++)
            {
                if((! TacticsGrid.GetInstance().CanTraverse(adjPts[i])) && (!adjPts[i].Equals(end))) {continue;}
                double dist = DistanceBetween(adjPts[i], end);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = adjPts[i];
                }
            }
            if (!TacticsGrid.GetInstance().CanTraverse(best)) {
                if (! best.Equals(end))
                    { throw new UnreachableDestinationException(); }
            }
            if (bestDist > DistanceBetween(current, end)) { throw new UnreachableDestinationException(); }
            return best;
        }

        private Point[] getAdjacentPoints(Point current)
        {
            return new Point[]{new Point(current.X-1, current.Y),
                                        new Point(current.X+1, current.Y),
                                        new Point(current.X, current.Y-1),
                                        new Point(current.X, current.Y+1)};
           
        }

        public Stack<Point> FindPath()
        {
            openSet.Add(start);
            OpenNode(start);
            try
            {
                AttemptDirectPath();
                FigurePath();
            } catch (UnreachableDestinationException) {
                // Do nothing
            }
            return ReconstructPath();
        }
        private void FigurePath()
        {
            HashSet<Node> open = openSet;
            HashSet<Node> closed = closedSet;
            Console.WriteLine("1 Figure Path BEGIN");
            while (! PathComplete())
            {
                OpenNode(PickNextNode());
            }
            Console.WriteLine("6 Figure Path END");
        }

        private Node PickNextNode()
        {
            Console.WriteLine("2 Pick Next Node BEGIN");
            if (openSet.Count == 0) { throw new UnreachableDestinationException();}
            Node bestNode = null;
            foreach (Node n in openSet)
            {
                if (bestNode == null)
                {
                    bestNode = n;
                }
                else if (n.fScore < bestNode.fScore)
                {
                    bestNode = n;
                }
            }
            Console.WriteLine("3 Pick Next Node END");
            return bestNode; // will be null for an empty set
        }

        private bool PathComplete()
        {
            return current.pos.Equals(end);
        }

        private void OpenNode(Node node)
        {
            current = node;
            openSet.Remove(node);
            closedSet.Add(node);
            if (!node.Impassable() || node == start) // don't add new open set nodes for blocked paths. 
            {
                AddToOpenSet(node.GetAdjacentNodes());
            }

        }

        private void AddToOpenSet(List<Node> newNodes)
        {
            foreach (Node node in newNodes)
            {
                if (!(openSet.Contains(node) || closedSet.Contains(node)))
                {
                    openSet.Add(node);
                }
            }
        }

        private void AttemptDirectPath()
        {
            while ((!PathComplete()) && (!current.Impassable()))
            {
                List<Node> options = current.GetAdjacentNodes();
                if (options.Count == 0) { break; }
                AddToOpenSet(options);
                OpenNode(selectDirectNode(options));
            } 
        }

        private Node selectDirectNode(List<Node> nodes)
        {
            Node best = nodes[0];
            double bestDistance = DistanceBetween(best.pos, end);
            for (int i = 1; i < nodes.Count; i++)
            {
                double dist = DistanceBetween(nodes[i].pos, end);
                if (dist < bestDistance)
                {
                    best = nodes[i];
                    bestDistance = dist;
                }
            }
            return best;    
        }

        private Stack<Point> ReconstructPath()
        {
            Stack<Point> finalPath = new Stack<Point>();
            while (current != start)
            {
                finalPath.Push(current.pos);
                current = current.cameFrom;
            }
            return finalPath;
        }


        public static double DistanceBetween(Point a, Point b)
        {
            double c = Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2);
            return Math.Sqrt(c);
        }
    }

    public class Node
    {
        public Point pos, endPt;
        public Node cameFrom;
        public int gScore;   // cost of getting to this node from the start
        public double fScore;             // heuristic distance from end.
        public Node(Point pos, Point endPt, Node cameFrom)
        {
            this.pos = pos;
            this.endPt = endPt;
            this.cameFrom = cameFrom;
            calculateGScore();
            calculateFScore();
        }

        protected Node() {} 

        private void calculateGScore()
        {
            gScore = cameFrom.gScore + 1;
        }

        protected void calculateFScore()
        {
            fScore = gScore + AStar.DistanceBetween(pos, endPt);
        }

        public List<Node> GetAdjacentNodes()
        {
            List<Node> nodes = new List<Node>();
            nodes.Add(new Node(new Point(pos.X-1, pos.Y), endPt, this));
            nodes.Add(new Node(new Point(pos.X + 1, pos.Y), endPt, this));
            nodes.Add(new Node(new Point(pos.X, pos.Y - 1), endPt, this));
            nodes.Add(new Node(new Point(pos.X, pos.Y + 1), endPt, this));
            //RemoveBlockedPaths(nodes);
            return nodes;
        }

        private void RemoveBlockedPaths(List<Node> nodes)
        {
            foreach (Node n in nodes)
            {
                if (!TacticsGrid.GetInstance().CanTraverse(n.pos))
                {
                    nodes.Remove(n);
                }
            }
        }

        public bool Impassable()
        {
            return !TacticsGrid.GetInstance().CanTraverse(pos);
        }

        public override int GetHashCode()
        {
            return pos.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            try
            {
                Node node = (Node)obj;
                return node.pos.Equals(this.pos);
            }
            catch (InvalidCastException)
            {
                return false;
            }
 	        return pos.Equals(obj);
        }
    }

    public class StartNode : Node
    {
        public StartNode(Point pos, Point endPt)
        {
            this.pos = pos;
            this.endPt = endPt;
            gScore = 0;
            calculateFScore();
        }
    }


    public class UnreachableDestinationException : Exception
    {
        public UnreachableDestinationException()
            : base("you cannot reach the selected end point from the given start point") { }
    }
}
