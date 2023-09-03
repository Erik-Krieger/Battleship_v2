using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships
{
    public enum ShipType
    {
        None = 0,
        Carrier = 1,
        Battleship = 2,
        Submarine = 3,
        Destroyer = 4,
        PatrolBoat = 5,
    }

    public enum HitType
    {
        None,
        Hit,
        Repeat,
        Sunk,
    }

    public abstract class Ship
    {
        protected private ShipType m_Type = ShipType.None;

        private Orientation m_Orientation = Orientation.Horizontal;
        private bool m_Reversed = false;

        public List<Position> Cells { get => m_Cells; private set => m_Cells = value; }
        private List<Position> m_Cells;

        public int Length { get => m_Length; }
        private int m_Length;

        public int XPos { get => m_Position.X; }
        public int YPos { get => m_Position.Y; }

        public char Letter { get => m_Letter; }
        private char m_Letter;

        public int HitCount { get => m_HitCount ; private set => m_HitCount = value; }
        private int m_HitCount = 0;

        public Position Location { get => m_Position; set => m_Position = value; }
        private Position m_Position = new Position();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theLetterRepresenation"></param>
        /// <param name="theLength"></param>
        public Ship( char theLetterRepresenation, int theLength )
        {
            m_Letter = theLetterRepresenation;
            m_Length = theLength;
            Cells = new List<Position>(m_Length);

            for (int anIdx = 0; anIdx < m_Length; anIdx++)
            {
                m_Cells.Add(new Position());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thePosition"></param>
        /// <param name="theOrientation"></param>
        /// <param name="isReversed"></param>
        public void SetShipValues(Position thePosition, Orientation theOrientation, bool isReversed = false)
        {
            Location.SetValuesFrom(thePosition);
            m_Orientation = theOrientation;
            m_Reversed = isReversed;

            // Set the cells of the ship.
            setShipCells();
        }

        /// <summary>
        /// Initialized the values of the ships grid cells.
        /// </summary>
        private void setShipCells()
        {
            for (int anIdx = 0; anIdx < m_Length; anIdx++)
            {
                if (IsHorizontal())
                {
                    Cells[anIdx].X = Location.X + anIdx;
                    Cells[anIdx].Y = Location.Y;
                }
                else
                {
                    Cells[anIdx].X = Location.X;
                    Cells[anIdx].Y = Location.Y + anIdx;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theXPos"></param>
        /// <param name="theYPos"></param>
        /// <param name="theLength"></param>
        /// <param name="theOrientation"></param>
        /// <returns></returns>
        private static bool isLegalPosition( int theXPos, int theYPos, int theLength, Orientation theOrientation )
        {
            if ( theOrientation == Orientation.Horizontal )
            {
                return ( theXPos >= 0 && theYPos + theLength <= 10 && theYPos >= 0 && theYPos < 10 );
            }
            else
            {
                return ( theXPos >= 0 && theXPos < 10 && theYPos >= 0 && theYPos + theLength <= 10 );
            }
        }

        /// <summary>
        /// Returns true if the Position is inside the ships hitbox
        /// </summary>
        /// <param name="thePosition"></param>
        /// <param name="isPlacementOnly"></param>
        /// <returns></returns>
        public HitType IsHit( Position thePosition )
        {
            // Iterate through all cells of the ship.
            foreach ( var aCell in m_Cells )
            {
                // These matching means, that they share the same coordinates.
                // We only want to mark is as a hit, if that cell hasn't been hit before.
                if (thePosition == aCell )
                {
                    // Return HitType.Repeat, if the cell has been hit before.
                    if (aCell.WasHit) return HitType.Repeat;

                    // Mark both cells as hit.
                    // aCell -> for counting unique hits on the ship
                    aCell.WasHit = true;
                    // thePosition -> for the tracking of the Enemy AI
                    thePosition.WasHit = true;

                    return HitType.Hit;
                }
            }

            return HitType.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theShip"></param>
        /// <returns></returns>
        public bool IntersectsWith(Ship theShip)
        {
            return this.Cells.Intersect(theShip.Cells).ToList().Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSunk()
        {
            return m_Cells.TrueForAll( (e) => e.WasHit );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsHorizontal()
        {
            return m_Orientation == Orientation.Horizontal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsVertical()
        {
            return m_Orientation == Orientation.Vertical;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool NotPlaced()
        {
            return !Location.IsValid();
        }

        /// <summary>
        /// Converts the Ship into an 12 bit representation
        /// bit 0-2  -> The Ship Type
        /// bit 3-9  -> The origin cell of the Ship
        /// bit 10   -> The orientation of the ship i.e. 0 = horizontal, 1 = vertical
        /// bit 11   -> Is flipped i.e. 0 = no, 1 = yes
        /// </summary>
        /// <returns>a bit field, storing the ships state</returns>
        public ushort ToBitField()
        {
            int aBitField = 0;

            aBitField |= ((int)m_Type << 13);

            // Get the cell index of the shpi origin
            // Top Left = 0
            // Bottom Right = 99
            int aCellIndex = Location.GetCellIndex();
            // Set bits 3 - 9 to the cell index.
            aBitField |= (aCellIndex << 6);

            // If the ship is vertical set bit 10 to 1.
            if (IsVertical())
            {
                aBitField |= (1 << 5);
            }

            // If the Ship is reversed, set bit 11 to 1.
            if (m_Reversed)
            {
                aBitField |= (1 << 4);
            }

            return (ushort)aBitField;
        }

        /// <summary>
        /// Sets the Ships values from a bit field passed in as the first argument
        /// They map as follows:
        /// bit 0 - 2 -> the ship type
        /// bit 3 - 9 -> the origin cell of the ship
        /// bit 10    -> the orientation of the ship, where 0 = horizontal and 1 = vertical
        /// bit 11    -> if the ship is reversed
        /// </summary>
        /// <param name="theBitField">A ushort storing the ships data</param>
        /// <exception cref="ArgumentException">Is thrown, when the type in the bitField does not match the ships type</exception>
        public void FromBitField(ushort theBitField)
        {
            int aBitField = (int)theBitField;

            // Extracting bit 0 - 2 into the type.
            ShipType aType = (ShipType)((aBitField >> 13) & 0b111);

            if (aType != m_Type)
            {
                throw new ArgumentException("The ship type in the bit field, does not match the type of the ship", nameof(theBitField));
            }

            // Extracting bit 3 - 9 into a cell index.
            int aCellIndex = ((aBitField >> 6) & 0b1111111);
            Location.FromCellIndex(aCellIndex);


            // Extracting bit 10 into m_Orientation.
            m_Orientation = ((aBitField >> 5) & 0b1) == 1 ? Orientation.Vertical : Orientation.Horizontal;

            // Extract bit 11 into m_Reversed.
            m_Reversed = ((aBitField >> 4) & 0b1) == 1;

            // Initialize the Ships grid cells.
            setShipCells();
        }

        public override string ToString()
        {
            string o = m_Orientation == Orientation.Horizontal ? "H" : "V";
            string s = $"{Letter}-({Location.ToString()})-{o}-{m_Reversed.ToString()}";

            m_Cells.ForEach((c) => { s += '-' + c.ToString(); });

            return s;
        }
    }
}
