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
            /*MovementPath path = MovementPath.FindPath(new Point(1, 1), new Point(8, 2));
            Stack<Point> stack = path.GetPath();
            Point expected = new Point(8, 2);
            Point actual = stack.Pop();
            Console.WriteLine("expected X " + expected.X + ", Y " + expected.Y + "\n"
                + "got X " + actual.X + ", Y " + actual.Y);
            Assert.IsTrue(expected.Equals(actual));*/
            Stack<Point> stack = new AStar(new Point(1, 1), new Point(8, 2)).FindPath();
            Point actual = new Point(0, 0);
            while (stack.Count > 0)
            {
                actual = stack.Pop();
            }
            Point expected = new Point(8, 2);
            Assert.IsTrue(expected.Equals(actual));
        }
        [TestMethod]
        public void FindsShortestPath()
        {
            Stack<Point> path = new AStar(new Point(1, 1), new Point(8, 2)).FindPath();
            int expected = 8;
            int actual = path.Count;
            Assert.IsTrue(expected == actual);
        }
        [TestMethod]
        public void NavigatesArroundWallsLengthCheck()
        {
            Stack<Point> path = new AStar(new Point(7, 8), new Point(8, 12)).FindPath();
            int expected = 11;
            int actual = path.Count;
            Assert.IsTrue(expected == actual, "you got " + actual + "instead");
        }
         [TestMethod]
        public void NavigatesArroundWallsResultCheck()
        {
            Stack<Point> path = new AStar(new Point(7, 8), new Point(8, 12)).FindPath();
            Point expected = new Point(8, 12);
            Point actual = path.Pop();
            while(path.Count > 0)
            {
                actual = path.Pop();
            }
            Assert.IsTrue(expected == actual, "you got "+actual+"instead");
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
            Assert.IsTrue(path.IsComplete());

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
            Assert.IsFalse(path.IsComplete());
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
        [TestMethod]
        public void DistanceBetweenPoints0()
        {
            MovementPath path = new MovementPath(new Point(0, 0), new Point(5, 0));
            double expected = 5;
            double actual = path.DistanceToEnd(new Point(0, 0));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }

        [TestMethod]
        public void DistanceBetweenPoints1()
        {
            MovementPath path = new MovementPath(new Point(0, 0), new Point(0, 5));
            double expected = 5;
            double actual = path.DistanceToEnd(new Point(0, 0));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }

        [TestMethod]
        public void DistanceBetweenPoints3()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(-4, 4));
            double expected = 5.83095;
            double actual = path.DistanceToEnd(new Point(1, 1));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }
        [TestMethod]
        public void DistanceBetweenPoints4()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(4, 4));
            double expected = 4.24264;
            double actual = path.DistanceToEnd(new Point(1, 1));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }
        [TestMethod]
        public void DistanceBetweenPoints5()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(10, 2));
            double expected = 9.05539;
            double actual = path.DistanceToEnd(new Point(1, 1));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }
        [TestMethod]
        public void DistanceBetweenPoints6()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(8, 2));
            double expected = 7;
            double actual = path.DistanceToEnd(new Point(1, 2));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }

        [TestMethod]
        public void DistanceBetweenPoints7()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(8, 2));
            double expected = 6.08276;
            double actual = path.DistanceToEnd(new Point(2, 1));
            double delta = .0001;
            Assert.IsTrue(Math.Abs(expected - actual) < delta);
        }

        [TestMethod]
        public void TestNextDirectStepStraight()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(1, 5));
            Point p = path.GetNextDirectStep();
            Point expected = new Point(1, 2);
            Assert.IsTrue(expected.X == p.X);
            Assert.IsTrue(expected.Y == p.Y);
        }

        [TestMethod]
        public void TestNextDirectStepDiagonal()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(2, 5));
            Point p = path.GetNextDirectStep();
            Point expected = new Point(1, 2);
            Assert.IsTrue(expected.X == p.X);
            Assert.IsTrue(expected.Y == p.Y);
        }

        [TestMethod]
        public void TestNextDirectStepDiagonalTie()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(3, 3));
            Point p = path.GetNextDirectStep();
            Point expected1 = new Point(1, 2);
            Point expected2 = new Point(2, 1);
            Assert.IsTrue(expected1.X == p.X || expected2.X == p.X);
            Assert.IsTrue(expected1.Y == p.Y || expected2.Y == p.Y);
        }
        [TestMethod]
        public void TestNextDirectStepAsInShortestPathTest()
        {
            MovementPath path = new MovementPath(new Point(1, 1), new Point(8, 2));
            Point p = path.GetNextDirectStep();
            Point expected = new Point(2, 1);
            Assert.IsTrue(expected.X == p.X);
            Assert.IsTrue(expected.Y == p.Y);
        }

        [TestMethod]
        public void TestNodeEqualsSamePoint()
        {
            Node start = new StartNode(new Point(-1, -1), new Point(10, 10));
            Node node1 = new Node(new Point(1, 1), new Point(10, 10), start);
            Node node2 = new Node(new Point(1, 1), new Point(10, 10), start);
            Assert.IsTrue(node1.Equals(node2));
        }

        [TestMethod]
        public void TestNodeEqualsSelf()
        {
            Node start = new StartNode(new Point(-1, -1), new Point(10, 10));
            Node node = new Node(new Point(1, 1), new Point(10, 10), start);
            Assert.IsTrue(node.Equals(node));
        }

        [TestMethod]
        public void TestStartNodeEqualsSelf()
        {
            Node node = new StartNode(new Point(1, 1), new Point(10, 10));
            Assert.IsTrue(node.Equals(node));
        }
        [TestMethod]
        public void TestNodeNotEquals()
        {
            Node start = new StartNode(new Point(-1, -1), new Point(10, 10));
            Node node1 = new Node(new Point(1, 1), new Point(10, 10), start);
            Node node2 = new Node(new Point(1, 5), new Point(10, 10), start);
            Assert.IsFalse(node1.Equals(node2));
        }

    }
}
