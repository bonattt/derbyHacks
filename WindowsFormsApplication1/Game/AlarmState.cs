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
        public abstract Point GetDestination(EnemyUnit e);
        public abstract void NextDestination(EnemyUnit e);

        public abstract AlarmState SpotPlayerAt(Point p);

        public abstract AlarmState PlayerSpottedElseWhere(Point p);

        public abstract void DrawAt(Graphics g, Point p);

    }
}
