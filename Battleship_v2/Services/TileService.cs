using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Battleship_v2.Utility
{
    public enum TileType
    {
        Water,
        Miss,
        Hit,
        Carrier,
        Battleship,
        Submarine,
        Destroyer,
        PatrolBoat,
    }

    public enum TileOrientation
    {
        Up,
        Right,
        Down,
        Left,
    }

    public struct Tile
    {
        public TileType Type;
        public TileOrientation Orientation;
        public byte[] Data;
    }

    public static class TileService
    {
        public static string Water = "../../Resources/dev_art/blue.png";
        public static string Miss = "../../Resources/dev_art/light_gray.png";
        public static string Hit = "../../Resources/dev_art/red.png";
        public static string Carrier = "../../Resources/dev_art/gray_c.png";
        public static string Battleship = "../../Resources/dev_art/gray_b.png";
        public static string Submarine = "../../Resources/dev_art/gray_s.png";
        public static string Destroyer = "../../Resources/dev_art/gray_d.png";
        public static string PatrolBoat = "../../Resources/dev_art/gray_p.png";

        private static string pathToWater = "../../Resources/dev_art/blue.png";
        private static string pathToMiss = "../../Resources/dev_art/light_gray.png";
        private static string pathToHit = "../../Resources/dev_art/red.png";
        private static string pathToCarrier = "../../Resources/dev_art/gray_c.png";
        private static string pathToBattleship = "../../Resources/dev_art/gray_b.png";
        private static string pathToSubmarine = "../../Resources/dev_art/gray_s.png";
        private static string pathToDestroyer = "../../Resources/dev_art/gray_d.png";
        private static string pathToPatrolBoat = "../../Resources/dev_art/gray_p.png";

        private static List<Tile> m_TileBuffer = new List<Tile>(20);

        public static byte[] GetTile(TileType theType, TileOrientation theOrientation = TileOrientation.Up)
        {
            foreach (Tile aTile in m_TileBuffer)
            {
                if (aTile.Type == theType && aTile.Orientation == theOrientation)
                {
                    return aTile.Data;
                }
            }

            string aPath;

            switch (theType)
            {
                case TileType.Water:
                    aPath = pathToWater;
                    break;
                case TileType.Miss:
                    aPath = pathToMiss;
                    break;
                case TileType.Hit:
                    aPath = pathToHit;
                    break;
                case TileType.Carrier:
                    aPath = pathToCarrier;
                    break;
                case TileType.Battleship:
                    aPath = pathToBattleship;
                    break;
                case TileType.Submarine:
                    aPath = pathToSubmarine;
                    break;
                case TileType.Destroyer:
                    aPath = pathToDestroyer;
                    break;
                case TileType.PatrolBoat:
                    aPath = pathToPatrolBoat;
                    break;
                default:
                    throw new InvalidDataException("The Tile type was invalid");
            }

            Image anImage = Bitmap.FromFile(aPath);
            MemoryStream aMemoryStream = new MemoryStream();
            anImage.Save(aMemoryStream, ImageFormat.Png);
            byte[] aByteArray = aMemoryStream.ToArray();

            m_TileBuffer.Add(new Tile
            {
                Type = theType,
                Orientation = theOrientation,
                Data = aByteArray
            });

            return aByteArray;
        }
    }
}
