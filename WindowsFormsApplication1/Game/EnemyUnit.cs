using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    class EnemyUnit : Unit
    {
        public override Color GetColor()
        {
            if (Selected)
            {
                return Color.DarkRed;
            }
            return Color.Crimson;
        }
    }
}
