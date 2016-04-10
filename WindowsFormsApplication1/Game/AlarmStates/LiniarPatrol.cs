using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    class LinearPatrol : Unaware
    {
        private bool useMain;
        private Stack<Point> mainRoute, returnRoute;
        public LinearPatrol(Stack<Point> route)
        {
            if (route.Count == 0)
            {
                throw new Exception();
            }
            mainRoute = route;
            useMain = true;
        }

        public override Point GetDestination(EnemyUnit e)
        {
            return Peek();
        }

        public override void NextDestination(EnemyUnit e)
        {
            try
            {
                if (useMain)
                {
                    Swap(mainRoute, returnRoute);
                }
                else
                {
                    Swap(returnRoute, mainRoute);
                }
            }
            catch (InvalidOperationException)
            {
                useMain = !useMain;
            }
        }


        private Point Peek()
        {
            if (useMain)
            {
                return mainRoute.Peek();
            }
            return returnRoute.Peek();
        }

        private Point Swap(Stack<Point> one, Stack<Point> two)
        {
            two.Push(one.Pop());
            return two.Peek();
        }
    }
}
