using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    public class CirculatrPatrol : Unaware
    {

        private Queue<Point> route;

        public CirculatrPatrol(Queue<Point> route)
        {
            this.route = route;
        }

        public override Point GetDestination()
        {
            return route.Peek();    
        }
        public override void NextDestination()
        {
            Point p = route.Dequeue();
            route.Enqueue(p);
        }

    }
}
