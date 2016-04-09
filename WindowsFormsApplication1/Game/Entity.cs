using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    public abstract class Entity
    {
        protected bool opaque;    // opaque (opaque = true) Entities block LoS through space they occupy, transparent entities do not (opaque = false)
        protected bool blocking;  // blocking = false: can attack through space, blocking = true cannot attack through space
        public abstract void DrawAt(Graphics g, Point p);

    }
}
