using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class PatrolBoat : Ship
    {
        const int LENGTH = 2;

        public PatrolBoat() : base(TileService.GetTile(TileType.PatrolBoat), LENGTH)
        {
            m_Type = ShipType.PatrolBoat;
        }
    }
}
