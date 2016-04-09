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
        private Dictionary<Point, Entity> entities;
        public TacticsGrid(int width, int height)
        {
            ctor(new Size(width, height));
        }
        public TacticsGrid(Size size)
        {
            ctor(size);
        }

        private void ctor(Size size)
        {
            this.Size = size;
            entities = new Dictionary<Point, Entity>();
            PopulateOuterWall();
            PopulateDefaultMap();
            Console.WriteLine("x/y = " + this.Width + "/" + this.Height);

//          InitializeComponent();
        }

        private void PopulateDefaultMap()
        {
            entities.Add(new Point(5, 5), new PlayerUnit());
            entities.Add(new Point(7, 7), new EnemyUnit());
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

        private void DrawGrid(Graphics g)
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
            Graphics g = e.Graphics;
            base.OnPaint(e);
            DrawGrid(g);
            DrawEntities(g);
        }

        private void DrawEntities(Graphics g)
        {
            foreach (Point p in entities.Keys)
            {
                Entity e = entities[p];
                e.DrawAt(g, ConvertPointToGraphics(p));
            }
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
    }
}
