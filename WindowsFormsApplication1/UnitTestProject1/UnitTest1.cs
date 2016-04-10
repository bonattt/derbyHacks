using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Game;
using System.Drawing;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class PathfindingTest
    {

        static PathfindingTest()
        {
            TacticsGrid grid = TacticsGrid.GetInstance();
            grid.AddEntity(new Point(5, 10), new Wall());
            grid.AddEntity(new Point(6, 10), new Wall());
            grid.AddEntity(new Point(7, 10), new Wall());
            grid.AddEntity(new Point(8, 10), new Wall());
            grid.AddEntity(new Point(9, 10), new Wall());
            grid.AddEntity(new Point(10, 10), new Wall());
            grid.AddEntity(new Point(11, 10), new Wall());
            grid.AddEntity(new Point(12, 10), new Wall());
            grid.AddEntity(new Point(13, 10), new Wall());
            grid.AddEntity(new Point(14, 10), new Wall());
            grid.AddEntity(new Point(15, 10), new Wall());
            grid.AddEntity(new Point(16, 10), new Wall());
            grid.AddEntity(new Point(17, 10), new Wall());
            grid.AddEntity(new Point(18, 10), new Wall());
            grid.AddEntity(new Point(19, 10), new Wall());
            grid.AddEntity(new Point(20, 10), new Wall());
        }

        [TestMethod]
        public void ReachesCorrectEndpoint()
        {
            MovementPath path = MovementPath.FindPath(new Point(1, 1), new Point(10, 2));
            Stack<Point> stack = path.GetPath();
            Assert.Equals(new Point(10, 2), stack.Pop());
        }
        [TestMethod]
        public void FindsShortestPath()
        {
            MovementPath path = MovementPath.FindPath(new Point(1, 1), new Point(10, 2));
            Assert.Equals(11, path.GetLength());
        }
        [TestMethod]
        public void NavigatesArroundWallsLengthCheck()
        {
            MovementPath path = MovementPath.FindPath(new Point(1, 1), new Point(10, 2));
            Assert.Equals(11, path.GetLength());
        }
        [TestMethod]
        public void PathReachesEndTrue()
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(2, 1));
            stack.Push(new Point(3, 1));
            Point end = new Point(4, 1);
            stack.Push(end);
            MovementPath path = new MovementPath(end, stack);
            Assert.IsTrue(path.PathReachesEnd());

        }
        [TestMethod]
        public void PathReachesEndFalse()
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(2, 1));
            stack.Push(new Point(3, 1));
            stack.Push(new Point(4, 1));
            Point end = new Point(5, 5);
            MovementPath path = new MovementPath(end, stack);
            Assert.IsFalse(path.PathReachesEnd());
        }

        [TestMethod]
        public void SelectBestPath()
        {
            Stack<Point> big_stack = new Stack<Point>();
            Stack<Point> med_stack = new Stack<Point>();
            Stack<Point> sml_stack = new Stack<Point>();

            big_stack.Push(new Point(1, 1));
            big_stack.Push(new Point(2, 2));
            big_stack.Push(new Point(4, 3));
            big_stack.Push(new Point(2, 2));
            big_stack.Push(new Point(4, 3));

            med_stack.Push(new Point(1, 1));
            med_stack.Push(new Point(2, 2));
            med_stack.Push(new Point(2, 2));

            sml_stack.Push(new Point(1, 1));
            List<MovementPath> list = new List<MovementPath>();
            list.Add(new MovementPath(new Point(10, 10), sml_stack));
            list.Add(new MovementPath(new Point(10, 10), med_stack));
            list.Add(new MovementPath(new Point(10, 10), big_stack));

            int expected = sml_stack.Count;
            int actual = MovementPath.SelectBestPath(list).GetLength();
            Assert.IsTrue(expected == actual);
        }
    }
}
