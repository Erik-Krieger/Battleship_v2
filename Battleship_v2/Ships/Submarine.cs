using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class Submarine : Ship
    {
        const int LENGTH = 3;

        public Submarine() : base(TileService.GetTile(TileType.Submarine), LENGTH)
        {
            m_Type = ShipType.Submarine;
        }
    }
}
