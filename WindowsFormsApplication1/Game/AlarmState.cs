using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public abstract class AlarmState
    {
        public abstract Point GetDestination();
        public abstract void NextDestination();

        public abstract AlarmState SpotPlayerAt(Point p);

        public abstract void DrawAt(Graphics g, Point p);

    }
}
