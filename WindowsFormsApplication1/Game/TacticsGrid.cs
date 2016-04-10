using Game.AlarmStates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    //public partial class TacticsGrid : Form
    public class TacticsGrid : Form
    {
        public static readonly int BOX_SIZE = 50;

        private static readonly Size DEFAULT_FULL_SCREEN = new Size(1920, 1080);

        private static TacticsGrid instance;
        
        private MovementPath path;
        private Point selected;
        private Graphics g;
        private List<PlayerUnit> playerUnits = new List<PlayerUnit>();
        private List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
        private Dictionary<Point, Entity> entities;
        
        protected TacticsGrid(Size size)
        {
            ctor(size);
        }
        public void AddEntity(Point p, Entity e)
        {
            entities.Add(p, e);
        }

        public static TacticsGrid GetInstance()
        {
            if (instance == null)
            {
                instance = new TacticsGrid();
            }
            return instance;
        }

        protected TacticsGrid()
        {
            ctor(DEFAULT_FULL_SCREEN);
        }

        private void EnemyTurn()
        {
            foreach (EnemyUnit e in enemyUnits)
            {
                MoveEnemy(GetEntityPosition(e), e);
            }
            Refresh();
        }

        public Point GetEntityPosition(Entity e)
        {
            foreach (Point p in entities.Keys)
            {
                if (entities[p] == e)
                {
                    return p;
                }
            }
            throw new Exception("the Dictionary did not contain the requested entity");
        }

        private void MoveEnemy(Point enPos, EnemyUnit eu)
        {
            int moves = eu.GetMovementSpeed();
            Stack<Point> path = GetMovementPath(enPos, eu);
            Point oldPoint;
            Point newPoint = enPos;
            while (moves-- > 0)
            {
                if (path.Count == 0) { break; }
                oldPoint = newPoint;
                newPoint = path.Pop();
                if (this.entities.ContainsKey(newPoint))
                { 
                    LookForPlayers(newPoint, eu);
                    break;
                }
                entities.Remove(oldPoint);
                entities.Add(newPoint, eu);
                eu.MoveUnit(oldPoint, newPoint);
                LookForPlayers(newPoint, eu);
            }
            if (path.Count == 0) { eu.state.NextDestination();}
        }

        private Stack<Point> GetMovementPath(Point pos, EnemyUnit eu)
        {
            Point destination = eu.state.GetDestination();
            AStar asr = new AStar(pos, destination);
            return asr.FindPath();
        }
        public void LookForPlayers(Point enPos, EnemyUnit eu)
        {
            foreach (PlayerUnit unit in playerUnits)
            {
                Point pPos = GetEntityPosition(unit);
                AStar astar = new AStar(enPos, pPos);
                bool clearLoS = astar.ClearLineOfSight();
                if (eu.InVisionCone(enPos, pPos) && clearLoS) 
                {
                    eu.SpotsPlayerAt(pPos);
                }
            }
        }

        private bool TurnOver()
        {
            selected = new Point(Int32.MinValue, Int32.MinValue);
            foreach (PlayerUnit u in playerUnits)
            {
                if (u.CanAct)
                {
                    return false;
                }
            }
            return true;
        }

        private void EndPlayerTurn()
        {

            foreach (PlayerUnit u in playerUnits)
            {
                u.CanAct = true;
            }
            Refresh();
            EnemyTurn();
        }

        private void ctor(Size size)
        {
            this.Text = "Cloak and Dagger";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Size = size;
            entities = new Dictionary<Point, Entity>();
            PopulateOuterWall();
            PopulateDefaultMap();

//          InitializeComponent();
        }
        /*protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int i = 1000;
            while (i-- > 0)
            {
                // TODO replace this with a Timer or some other alternative to 
                // thread.Sleep();
            }

            if (entities.ContainsKey(selected))
            {
                Entity ent = entities[selected];
                if (ent.CanMove())
                {
                    SetMovementPath();
                    Refresh();
                    Console.WriteLine("6 done refreshing after mouse move\n");
                    return;
                }
            }
            path = null;
        }*/

        private bool CanSee(Point p1, Point p2)
        {
            MovementPath path = MovementPath.FindDirectPath(p1, p2);
            return path.IsObstructed();
        }

        private void SetMovementPath()
        {
            path = MovementPath.FindPath(selected, ConvertPointToGrid(MousePosition));
            Console.WriteLine("1 DONE FINDING PATH");
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            int z = Control.MousePosition.X;

            int x = ConvertIntToGrid(MousePosition.X);
            int y = ConvertIntToGrid(MousePosition.Y);
            Point p = new Point(x, y);
            DeselectAll();
            if(! entities.ContainsKey(p)) 
            {
                TryToMoveTo(p);
            }
            else
            {
                Entity ent = entities[p];
                ent.ClickOn();
            }

            selected = p;
            Refresh();
            if (TurnOver())
            {
                EndPlayerTurn();
            }
        }

        private void TryToMoveTo(Point p)
        {
            if (entities.ContainsKey(selected))
            {
                try
                {
                    PlayerUnit unit = (PlayerUnit) entities[selected];
                    if (!unit.CanAct)
                    {
                        return;
                    }
                    entities.Remove(selected);
                    entities.Add(p, unit);
                    unit.CanAct = false;
                }
                catch (InvalidCastException)
                {
                    // do nothing
                }
            }
        }

        public bool CanTraverse(Point p)
        {
            if (!entities.ContainsKey(p))
            {
                return true;
            }
            Entity ent = entities[p];
            return ent.CanTraverseEntity();
        }

        private void PopulateDefaultMap()
        {
            PlayerUnit p = new PlayerUnit();
            entities.Add(new Point(3, 3), p);
            playerUnits.Add(p);
            /*
            p = new PlayerUnit();
            entities.Add(new Point(10, 3), p);
            playerUnits.Add(p);

            p = new PlayerUnit();
            p.CanAct = true;
            entities.Add(new Point(15, 3), p);
            playerUnits.Add(p);

            p = new PlayerUnit();
            p.CanAct = false;
            entities.Add(new Point(15, 15), p);
            playerUnits.Add(p);*/

            Queue<Point> patrolRoute = new Queue<Point>();
            patrolRoute.Enqueue(new Point(5, 10));
            patrolRoute.Enqueue(new Point(10, 10));
            patrolRoute.Enqueue(new Point(7, 4));

            EnemyUnit e = new EnemyUnit(new CirculatrPatrol(patrolRoute));
            entities.Add(new Point(7, 3), e);
            enemyUnits.Add(e);

            entities.Add(new Point(8, 8), new Wall());
            entities.Add(new Point(8, 9), new Wall());
            entities.Add(new Point(8, 10), new Wall());
            entities.Add(new Point(8, 11), new Wall());
            entities.Add(new Point(8, 12), new Wall());
            entities.Add(new Point(8, 13), new Wall());
            entities.Add(new Point(8, 14), new Wall());

            entities.Add(new Point(16, 8), new Wall());
            entities.Add(new Point(16, 9), new Wall());
            entities.Add(new Point(16, 10), new Wall());
            entities.Add(new Point(16, 11), new Wall());
            entities.Add(new Point(16, 12), new Wall());
            entities.Add(new Point(16, 13), new Wall());
            entities.Add(new Point(16, 14), new Wall());


            entities.Add(new Point(20, 14), new Wall());
            entities.Add(new Point(19, 14), new Wall());
            entities.Add(new Point(18, 14), new Wall());
            entities.Add(new Point(20, 16), new Wall());
            entities.Add(new Point(19, 16), new Wall());
            entities.Add(new Point(18, 16), new Wall());
            entities.Add(new Point(20, 15), new Wall());
            entities.Add(new Point(18, 15), new Wall());
        }

        private void PopulateOuterWall()
        {
            int xEdge = (this.Width / BOX_SIZE) - 1;
            int yEdge = (this.Height / BOX_SIZE) - 1;
            for (int x = 0; x <= xEdge; x += 1)
            {
                entities.Add(new Point(x, 0), new Wall());
                entities.Add(new Point(x, yEdge), new Wall());
            }
            for (int y = 1; y < yEdge; y += 1)
            {
                entities.Add(new Point(0, y), new Wall());
                entities.Add(new Point(xEdge, y), new Wall());
            }
        }

        private void DrawGrid()
        {
            Pen pen = new Pen(Color.Black);
            //horrizontal lines
            for (int y = BOX_SIZE; y < this.Height; y += BOX_SIZE)
            {
                g.DrawLine(pen, new Point(0, y), new Point(this.Width, y));
            }
            // verticle lines
            for (int x = BOX_SIZE; x < this.Width; x += BOX_SIZE)
            {
                g.DrawLine(pen, new Point(x, 0), new Point(x, this.Height));
            }
            pen.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Console.WriteLine("2 start paint");
            base.OnPaint(e);
            g = e.Graphics;
            DrawGrid();
            AddEntities();
            Console.WriteLine("3 writing movement path");
            PaintMovementPath();
            Console.WriteLine("5 done writing movement path");
            //highlightSelection();*/
        }

        private void PaintMovementPath()
        {
            if (path == null)
            {
                return;
            }
            Pen pen = GetPen();
            Stack<Point> points = path.GetPath();
            Point last = points.Pop();
            Point current = points.Pop();
            int DEBUG = 1;
            while (points.Count > 0)
            {
                Console.WriteLine("4 painting line #" + (DEBUG++));
                g.DrawLine(pen, last, current);
                last = current;
                current = points.Pop();
            }
            g.DrawLine(pen, last, current);
            pen.Dispose();
        }

        private Pen GetPen()
        {
            Pen pen;
            if (path.IsComplete())
            {
                pen = new Pen(Color.Green);
            }
            else
            {
                pen = new Pen(Color.Gray);
            }
            pen.Width = 3;
            return pen;
        }

        private void highlightSelection()
        {
            if (entities.ContainsKey(selected))
            {
                Pen pen = new Pen(Color.Green);
                pen.Width = 3;
                Rectangle bounds = new Rectangle(selected, new Size(BOX_SIZE*(3/2), BOX_SIZE*(3/2)));
                g.DrawEllipse(pen, bounds);
                pen.Dispose();
            }
        }

        private void AddEntities()
        {
            foreach (Point p in entities.Keys)
            {
                Entity e = entities[p];
                e.DrawAt(g, ConvertPointToGraphics(p));
            }
        }

        public static Point ConvertPointToGrid(Point p)
        {
            int newX = ConvertIntToGrid(p.X);
            int newY = ConvertIntToGrid(p.Y);
            return new Point(newX, newY);
        }

        public static Point ConvertPointToGraphics(Point p)
        {
            int newX = ConvertIntToGraphics(p.X);
            int newY = ConvertIntToGraphics(p.Y);
            return new Point(newX, newY);
        }

        public static int ConvertIntToGraphics(int val)
        {
            return val * BOX_SIZE;
        }

        public static int ConvertIntToGrid(int val)
        {
            return val / BOX_SIZE;
        }
        private void DeselectAll()
        {
            foreach (PlayerUnit u in playerUnits)
            {
                u.Selected = false;
            }
            foreach (EnemyUnit u in enemyUnits)
            {
                u.Selected = false;
            }
        }
    }
}
