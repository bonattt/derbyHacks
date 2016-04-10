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
        private Point lastSighting;
        private 

        public Hostile(Point p)
        {
            lastSighting = p;
        }

        public override Point GetDestination()
        {
            NextDestination();
            return lastSighting;
        }

        public override void NextDestination()
        {
            
        }

        public override AlarmState SpotPlayerAt(Point p)
        {
            lastSighting = p;
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
