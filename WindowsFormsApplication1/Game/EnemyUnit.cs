using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game
{
    class EnemyUnit : Unit
    {
        public override Color GetColor()
        {
            if (selected)
            {
                return Color.DarkRed;
            }
            return Color.Crimson;
        }
    }
}
