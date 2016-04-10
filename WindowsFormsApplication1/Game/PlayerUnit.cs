using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    class PlayerUnit : Unit
    {
        public bool CanAct;

        public PlayerUnit()
        {
            this.CanAct = true;
        }

        public override Color GetColor()
        {
            if (!CanAct)
            {
                return Color.Gray;
            }
            else if (Selected)
            {
                return Color.Green;
            }
            return Color.Blue;
        }

        public override bool IsPlayer()
        {
            return true;
        }
    }
}
