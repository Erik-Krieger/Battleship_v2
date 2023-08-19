using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships
{
    public abstract class Ship
    {
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

            for ( int anIdx = 0; anIdx < m_Length; anIdx++ )
            {
                if (IsHorizontal())
                {
                    Cells[anIdx].X = thePosition.X + anIdx;
                    Cells[anIdx].Y = thePosition.Y;
                }
                else
                {
                    Cells[anIdx].X = thePosition.X;
                    Cells[anIdx].Y = thePosition.Y + anIdx;
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
        public bool IsHit( Position thePosition )
        {
            //m_Cells.ForEach(cell => { Debug.WriteLine(cell); });

            foreach ( var aCell in m_Cells )
            {
                Debug.WriteLine($"Positions {thePosition} - {aCell}");
                // These matching means, that they share the same coordinates.
                // We only want to mark is as a hit, if that cell hasn't been hit before.
                if (thePosition == aCell )
                {
                    Debug.WriteLine("Matching");
                    //Debug.WriteLine("Cells colliding");
                    // Skip Iteration, if the cell was already hit.
                    if (aCell.WasHit) continue;

                    // Mark both cells as hit.
                    // aCell -> for counting unique hits on the ship
                    aCell.WasHit = true;
                    // thePosition -> for the tracking of the Enemy AI
                    thePosition.WasHit = true;

                    return true;
                }
            }

            return false;
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
    }
}
