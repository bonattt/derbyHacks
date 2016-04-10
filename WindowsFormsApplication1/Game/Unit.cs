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

        public bool Selected;

        public Unit()
            : base()
        {
            this.Selected = false;
        }

        public override void DrawAt(Graphics g, Point p)
        {
            SolidBrush brush = new SolidBrush(GetColor());
            Rectangle bounds = new Rectangle(p, new Size(TacticsGrid.BOX_SIZE, TacticsGrid.BOX_SIZE));
            g.FillEllipse(brush, bounds);
            brush.Dispose();
        }
        public override bool CanMove()
        {
            return true;
        }

        public override void ClickOn()
        {
            Selected = true;
        }
        public abstract Color GetColor();
    }
}
