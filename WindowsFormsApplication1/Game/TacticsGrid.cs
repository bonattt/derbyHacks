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
        private readonly int BOX_SIZE = 50;
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
//          InitializeComponent();
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
            base.OnPaint(e);
            Graphics g = e.Graphics;
            DrawGrid(g);
        }

        private void DrawEntities()
        {
            foreach (Point p in entities.Keys)
            {
                Entity e = entities[p];
            }
        }
    }
}
