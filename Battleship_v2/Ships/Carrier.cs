using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class Carrier : Ship
    {
        const int LENGTH = 5;

        public Carrier() : base(Tiles.Carrier, LENGTH)
        {
            m_Type = ShipType.Carrier;
        }
    }
}
