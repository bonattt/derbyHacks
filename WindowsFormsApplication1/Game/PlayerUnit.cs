using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    class PlayerUnit : Unit
    {
        
        public override Color GetColor()
        {
            if (selected)
            {
                return Color.Green;
            }
            return Color.Blue;
        }
    }
}
