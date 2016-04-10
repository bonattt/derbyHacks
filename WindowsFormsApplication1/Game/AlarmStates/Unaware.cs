using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    public abstract class Unaware : AlarmState
    {
        public override AlarmState SpotPlayerAt(Point p)
        {
            return new Hostile(p);
        }
        public override void DrawAt(Graphics g, Point p) {} // do nothing

        public override AlarmState PlayerSpottedElseWhere(Point p)
        {
            return new Hostile(p, false);
        }
    }
}
