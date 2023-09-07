using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class Destroyer : Ship
    {
        const int LENGTH = 3;

        public Destroyer() : base(Tile.Destroyer, LENGTH)
        {
            m_Type = ShipType.Destroyer;
        }
    }
}
