using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public abstract class Entity : Form
    {
        protected bool opaque;    // opaque (opaque = true) Entities block LoS through space they occupy, transparent entities do not (opaque = false)
        protected bool blocking;  // blocking = false: can attack through space, blocking = true cannot attack through space

        public Entity()
        {
            this.opaque = true;
            this.blocking = true;
        }

        public Entity(bool opaque, bool blocking)
        {
            this.opaque = opaque;
            this.blocking = blocking;
        }
        
        public abstract void DrawAt(Graphics g, Point p);
        public virtual void ClickOn()
        {
            MessageBox.Show("You cannot interact with that!!");
        }

        public virtual bool IsPlayer()
        {
            return false;
        }

        public bool CanTraverseEntity()
        {
            return !blocking;
        }

        public virtual bool CanMove()
        {
            return false;
        }
    }
}
