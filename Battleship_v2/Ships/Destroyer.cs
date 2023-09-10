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

        public Destroyer() : base(LENGTH)
        {
            m_Type = ShipType.Destroyer;
        }

        protected private override void setTileSet()
        {
            TileSet = new List<byte[]>(LENGTH);

            TileOrientation anOrientation = IsHorizontal() ? TileOrientation.Left : TileOrientation.Up;

            if (Reversed)
            {
                if (anOrientation == TileOrientation.Left)
                {
                    anOrientation = TileOrientation.Right;
                }
                else if (anOrientation == TileOrientation.Up)
                {
                    anOrientation = TileOrientation.Down;
                }
            }

            TileSet.Add(TileService.GetTile(TileType.Destroyer, anOrientation));
            TileSet.Add(TileService.GetTile(TileType.Destroyer, anOrientation));
            TileSet.Add(TileService.GetTile(TileType.Destroyer, anOrientation));

            if (Reversed)
            {
                TileSet.Reverse();
            }
        }
    }
}
