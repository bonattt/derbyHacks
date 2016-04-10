using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    class Stationary : Unaware
    {
        private Point startPoint;
        
        public Stationary(Point startPoint)
        {
            this.startPoint = startPoint;
        }

        public override Point GetDestination()
        {
            return startPoint;
        }

        public override void NextDestination(EnemyUnit e) { } // do nothing
    }
}
