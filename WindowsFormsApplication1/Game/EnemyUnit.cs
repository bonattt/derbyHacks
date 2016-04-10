using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class EnemyUnit : Unit
    {
        public static readonly int UP = 0;
        public static readonly int LEFT = 1;
        public static readonly int DOWN = 2;
        public static readonly int RIGHT = 3;

        public AlarmState state;
        private int orientation;

        public EnemyUnit(AlarmState state)
        {
            this.state = state;
            orientation = DOWN;
        }
        public override void DrawAt(Graphics g, Point p)
        {
            base.DrawAt(g, p);
            drawOrientation(g, p);
            DrawMovementLine(g, p);
            state.DrawAt(g, p);
        }

        public void SpotsPlayerAt(Point p)
        {
            MessageBox.Show("the enemy has spotted you!!");
            state = state.SpotPlayerAt(p);
        }

        private void drawOrientation(Graphics g, Point p)
        {
            Point[] points;
            if (orientation == UP)
            {
                points = new Point[] {new Point(p.X-10, p.Y+10), new Point(p.X+10, p.Y+10), new Point(p.X, p.Y-10)};
            }
            else if (orientation == LEFT)
            {
                points = new Point[] {new Point(p.X-10, p.Y-10), new Point(p.X-10, p.Y+10), new Point(p.X+10, p.Y)};
            }
            else if (orientation == DOWN)
            {
                points = new Point[] { new Point(p.X - 10, p.Y - 10), new Point(p.X + 10, p.Y - 10), new Point(p.X, p.Y + 10) };
            }
            else if (orientation == RIGHT)
            {
                points = new Point[] { new Point(p.X + 10, p.Y - 10), new Point(p.X + 10, p.Y + 10), new Point(p.X - 10, p.Y) };
            } 
            else
            {
                points = new Point[0];
            }
            Brush brush = new SolidBrush(Color.Gray);
            g.FillPolygon(brush, points);
            brush.Dispose();
        }

        
        private void DrawMovementLine(Graphics g, Point p)
        {
            Stack<Point> path = (new AStar(TacticsGrid.ConvertPointToGrid(p), state.GetDestination())).FindPath();
            int moves = GetMovementSpeed();
            Point current = p;
            Point lastPoint = new Point(Int32.MinValue, Int32.MinValue);
            Pen pen = new Pen(Color.Red);
            pen.Width = 3;
            //while (moves-- >= 0 && path.Count > 0)
            while(path.Count > 0)
            {
                lastPoint = current;
                current = path.Pop();
                g.DrawLine(pen, TacticsGrid.ConvertPointToGraphics(CenterPoint(lastPoint)), TacticsGrid.ConvertPointToGraphics(CenterPoint(current)));
            }
            pen.Dispose();
            pen = new Pen(Color.Yellow);
            pen.Width = 3;
            Rectangle bound = new Rectangle(TacticsGrid.ConvertPointToGraphics(lastPoint), new Size(50, 50));
            g.DrawEllipse(pen, bound);
            pen.Dispose();
        }

        private Point CenterPoint(Point p)
        {
            return new Point(p.X + 25, p.Y + 25);
        }

        public bool InVisionCone(Point myPos, Point targetPos)
        {
            if (orientation == UP)
            {
                if (targetPos.Y >= myPos.Y) { return false; }
                int xDif = Math.Abs(myPos.X - targetPos.X);
                int yDif = Math.Abs(myPos.Y - targetPos.Y);
                if (xDif > yDif) { return false; }
                return true;
            }
            else if (orientation == LEFT)
            {
                if (targetPos.X >= myPos.X) { return false; }
                int xDif = Math.Abs(myPos.X - targetPos.X);
                int yDif = Math.Abs(myPos.Y - targetPos.Y);
                if (xDif < yDif) { return false; }
                return true;
            }
            else if (orientation == DOWN)
            {
                if (targetPos.Y <= myPos.Y) { return false; }
                int xDif = Math.Abs(myPos.X - targetPos.X);
                int yDif = Math.Abs(myPos.Y - targetPos.Y);
                if (xDif > yDif) { return false; }
                return true;
            }
            else if (orientation == RIGHT)
            {
                if (targetPos.X <= myPos.X) { return false; }
                int xDif = Math.Abs(myPos.X - targetPos.X);
                int yDif = Math.Abs(myPos.Y - targetPos.Y);
                if (xDif < yDif) { return false; }
                return true;
            }
            else { throw new Exception("Impossible orientation of guard"); }
        }
        
        public override Color GetColor()
        {
            if (Selected)
            {
                return Color.DarkRed;
            }
            return Color.Crimson;
        }

        public int GetMovementSpeed()
        {
            return 4;
        }

        public void MoveUnit(Point oldPos, Point newPos) {
            // should be used each time 1 space is moved, then the guard looks for players,
            // then continues to move.
            if (oldPos.X > newPos.X)
            {
                orientation = RIGHT;
            }
            else if (oldPos.X < newPos.X)
            {
                orientation = LEFT;
            }
            else if (oldPos.Y > newPos.Y)
            {
                orientation = UP;
            }
            else if (oldPos.Y < newPos.Y)
            {
                orientation = DOWN;
            }
            else { throw new Exception("Enemy unit called Move without moving"); }
        }



    }
}
