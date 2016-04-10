using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game.AlarmStates
{
    

    public class Hostile : AlarmState
    {
        private Point intriguePt;
        private int lastSawPlayer;

        public Hostile(Point p)
        {
            intriguePt = p;
            lastSawPlayer = 0;
        }

        public Hostile(Point p, bool sawPlayer)
        {
            intriguePt = p;
            if (sawPlayer)
            {
                lastSawPlayer = 0;
            }
            else 
            {
                lastSawPlayer = Int32.MaxValue;
            }
               
        }

        public override Point GetDestination(EnemyUnit e)
        {
            lastSawPlayer -= 1;
            NextDestination(e);
            return intriguePt;
        }

        public override void NextDestination(EnemyUnit e)
        {
            if (lastSawPlayer > 1)
            {
                GuardCommunication com = GuardCommunication.GetInstance();
                com.getNewInvestigationPoint(e);
            }
        }

        public override AlarmState SpotPlayerAt(Point p)
        {
            lastSawPlayer = 0;
            intriguePt = p;
            return this;
        }

        public override AlarmState PlayerSpottedElseWhere(Point p)
        {
            if (lastSawPlayer > 3)
            {
                intriguePt = p;
            }
            return this;
        } 

        public override void DrawAt(Graphics g, Point p) 
        {
            Brush brush = new SolidBrush(Color.Black);
            DrawTriangle(g, p, brush);
            DrawCircle(g, p, brush);
            brush.Dispose(); 
        }

        private void DrawTriangle(Graphics g, Point p, Brush b)
        {
            Point[] points = new Point[] {new Point(p.X + 25, p.Y + 20), new Point(p.X + 20, p.Y + 5),
                            new Point(p.X+30, p.Y+5)};
            g.FillPolygon(b, points);
        }

        private void DrawCircle(Graphics g, Point p, Brush b)
        {
            p = new Point(p.X + 25, p.Y + 25);
            Rectangle bounds = new Rectangle(p, new Size(5, 5));
            g.FillEllipse(b, bounds);
        }

    }
}
