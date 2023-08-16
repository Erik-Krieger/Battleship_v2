using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using Battleship_v2.Utility;

namespace Battleship_v2.Ships
{
    public abstract class Ship
    {
        private Position m_Position;
        private Orientation m_Orientation = Orientation.Horizontal;
        private bool m_Reversed = false;
        private char m_Letter;
        private int m_Length;
        private int m_HitCount = 0;
        private List<Position> m_Cells;

        public int Length { get => m_Length; }
        public int XPos { get => m_Position.X; }
        public int YPos { get => m_Position.Y; }
        public char Letter { get => m_Letter; }
        public int HitCount { get => m_HitCount ; private set => m_HitCount = value; }
        public Position Location { get => m_Position; set => m_Position = value; }

        public Ship( char theLetterRepresenation, int theLength )
        {
            m_Letter = theLetterRepresenation;
            m_Length = theLength;
            m_Cells = new List<Position>(m_Length);
        }

        public void SetShipValues(Position thePosition, Orientation theOrientation, bool isReversed)
        {
            Location = thePosition;
            m_Orientation = theOrientation;
            m_Reversed = isReversed;

            m_Cells.Clear();

            /*m_Cells.Add( thePosition.Clone() );

            for ( int anIdx = 1; anIdx < m_Length; anIdx++ )
            {
                m_Cells.Add( m_Cells[m_Cells.Count - 1].GetNeighbour( theOrientation ) );
            }*/

            for ( int anIdx = 0; anIdx < m_Length; anIdx++ )
            {
                if ( theOrientation == Orientation.Horizontal )
                {
                    var p = new Position( thePosition.X + anIdx, thePosition.Y );
                    m_Cells.Add( p );
                }
                else
                {
                    var p = new Position( thePosition.X, thePosition.Y + anIdx );
                    m_Cells.Add( p );
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
                return ( theXPos >= 0 && theYPos + theLength < 10 && theYPos >= 0 && theYPos < 10 );
            }
            else
            {
                return ( theXPos >= 0 && theXPos < 10 && theYPos >= 0 && theYPos + theLength < 10 );
            }
        }

        // Returns true if the Position is inside the ships hitbox
        public bool IsHit( Position thePosition, bool isPlacementOnly = false )
        {
            //m_Cells.ForEach( ( E ) => { Debug.WriteLine( $"{E.X},{E.Y}" ); } );

            foreach ( var aCell in m_Cells )
            {
                // These matching means, that they share the same coordinates.
                // We only want to mark is as a hit, if that cell hasn't been hit before.
                if (thePosition == aCell && !aCell.WasHit )
                {
                    // If this happens during placement exit here.
                    if ( isPlacementOnly ) return true;

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

        public bool IsSunk()
        {
            return m_Cells.TrueForAll( ( aCell ) => aCell.WasHit ) ;
        }

        public bool IsHorizontal()
        {
            return m_Orientation == Orientation.Horizontal;
        }

        public bool NotPlaced()
        {
            return m_Cells.Count == 0;
        }
    }
}
