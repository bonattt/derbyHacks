using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public abstract class Unit : Entity
    {

        protected bool selected;
        public override void DrawAt(Graphics g, Point p)
        {
            SolidBrush brush = new SolidBrush(GetColor());
            Rectangle bounds = new Rectangle(p, new Size(TacticsGrid.BOX_SIZE, TacticsGrid.BOX_SIZE));
            g.FillEllipse(brush, bounds);
            brush.Dispose();
        }
        public abstract Color GetColor();
    }
}
