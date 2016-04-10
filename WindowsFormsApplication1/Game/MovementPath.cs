
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public class MovementPath
    {
        private static readonly int MAX_PATH_LENGTH = 20;
        private static readonly Object movementPathLock = new Object();

        private Point b;
        private Stack<Point> path;
        private bool complete;

        public MovementPath(Point a, Point b)
        {
            this.b = b;
            complete = false;
            this.path = new Stack<Point>();
            path.Push(a);
        }

        public MovementPath(Point b, Stack<Point> path)
        {
            this.b = b;
            this.path = path;
            complete = false;
        }


        public bool IsComplete()
        {
            return complete;
        }

        private static MovementPath FindPath(MovementPath current, int limit)
        {
            Console.WriteLine("finding path at length: " + current.GetLength());
            if (current.GetLength() == limit)
            {
                return current;
            }
            else if (current.PathReachesEnd())
            {
                current.complete = true;
                return current;
            }
            return TryNextSteps(current, limit);
        }
        private static MovementPath TryNextSteps(MovementPath current, int limit)
        {
            List<MovementPath> paths = new List<MovementPath>();
            Point[] pointsToTry = getPointsToTry(current.path.Peek());
            foreach (Point p in pointsToTry)
            {
                TrySingleStep(paths, current, p, limit); // mutates paths
            }
            MovementPath bestPath = SelectBestPath(paths);
            if (bestPath == null) {
                return current;
            }
            return bestPath;
        }

        private static Point[] getPointsToTry(Point last)
        {
            return new Point[]{new Point(last.X-1, last.Y),
                new Point(last.X+1, last.Y),
                new Point(last.X, last.Y-1),
                new Point(last.X, last.Y+1)};
        }

        private static void TrySingleStep(List<MovementPath> paths, MovementPath current, Point step, int limit)
        {
            if (current.path.Contains(step))
            {
                return;
            }
            if (!TacticsGrid.GetInstance().CanTraversePoint(step))
            {
                return;
            }
            MovementPath newPath = FindPath(current.AddPoint(step), limit);
            if (newPath != null)
            {
                paths.Add(newPath);
            }
        }

        public static MovementPath SelectBestPath(List<MovementPath> paths)
        {
            if (paths.Count == 0)
            {
                return null;
            }
            if (paths.Count == 1)
            {
                return paths[0];
            }
            MovementPath best = paths[0];
            foreach (MovementPath current in paths)
            {                
                if (current.GetLength() < best.GetLength() && (current.IsComplete() || !best.IsComplete()))
                {
                    best = current;
                }
            }
            return best;
        }

        public static MovementPath FindPath(Point a, Point b)
        {
            return FindPath(new MovementPath(a, b), MAX_PATH_LENGTH);
        }

        public int GetLength()
        {
            return path.Count;
        }

        public bool PathReachesEnd()
        {
            return path.Peek().Equals(b);
        }

        private MovementPath AddPoint(Point p)
        {
            Stack<Point> newPath = CopyStack();
            newPath.Push(p);
            return new MovementPath(b, newPath);
        }

        private Stack<Point> CopyStack()
        {
            Stack<Point> tempStack = new Stack<Point>();
            while(path.Count > 0)
            {
                tempStack.Push(path.Pop());
            }
            Stack<Point> newStack = new Stack<Point>();
            while (tempStack.Count > 0)
            {
                Point p = tempStack.Pop();
                newStack.Push(p);
                path.Push(p);
            }
            return newStack;
        }
        public Stack<Point> GetPath()
        {
            return path;
        }

    }
}
