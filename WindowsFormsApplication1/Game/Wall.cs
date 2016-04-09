using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public class Wall : Entity
    {
        private readonly Color DEFAULT_FILL = Color.Black;
        private Color color;
        public Wall(Color color)
        {
            ctor(color);
        }

        public Wall()
        {
            ctor(DEFAULT_FILL);
        }

        private void ctor(Color color)
        {
            this.color = color;
            this.blocking = true;
            this.opaque = false;
        }

        public override void DrawAt(Graphics g, Point p)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            Rectangle bounds = new Rectangle(p, new Size(TacticsGrid.BOX_SIZE, TacticsGrid.BOX_SIZE));
            g.FillRectangle(brush, bounds);
            brush.Dispose();
        }

    }
}
