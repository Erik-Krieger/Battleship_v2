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

        public Battleship() : base(LENGTH)
        {
            m_Type = ShipType.Battleship;
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

            TileSet.Add(TileService.GetTile(TileType.Battleship, anOrientation));
            TileSet.Add(TileService.GetTile(TileType.Battleship, anOrientation));
            TileSet.Add(TileService.GetTile(TileType.Battleship, anOrientation));
            TileSet.Add(TileService.GetTile(TileType.Battleship, anOrientation));

            if (Reversed)
            {
                TileSet.Reverse();
            }
        }
    }
}
