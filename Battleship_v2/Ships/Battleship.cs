using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class Battleship : Ship
    {
        const int LENGTH = 4;

        public Battleship() : base(Tiles.Battleship, LENGTH)
        {
            m_Type = ShipType.Battleship;
        }
    }
}
