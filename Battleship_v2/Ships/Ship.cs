using System.Windows.Controls;

namespace Battleship_v2.Ships
{
    public abstract class Ship
    {
        private int m_XPos = -1;
        private int m_YPos = -1;
        private Orientation m_Orientation = Orientation.Horizontal;
        private bool m_Reversed = false;
        private char m_Letter;
        private int m_Length;
        private int m_HitCount = 0;

        public int Length { get => m_Length; }
        public int XPos { get => m_XPos; }
        public int YPos { get => m_YPos; }
        public char Letter { get => m_Letter; }

        public Ship( char theLetterRepresenation, int theLength )
        {
            m_Letter = theLetterRepresenation;
            m_Length = theLength;
        }

        public void SetShipValues(int theXPos, int theYPos, Orientation theOrientation, bool isReversed)
        {
            m_XPos = theXPos;
            m_YPos = theYPos;
            m_Orientation = theOrientation;
            m_Reversed = isReversed;
        }

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

        public bool IsHit( int theXPos, int theYPos, bool isPlacementOnly = false )
        {
            if (!isPlacementOnly)
            {
                m_HitCount++;
            }

            if ( m_Orientation == Orientation.Horizontal )
            {
                return ( theXPos >= m_XPos && theXPos < m_XPos + m_Length && theYPos == m_YPos );
            }
            else
            {
                return ( theYPos >= m_YPos && theYPos < m_YPos + m_Length && theXPos == m_XPos );
            }
        }

        public bool IsSunk()
        {
            return m_HitCount >= m_Length;
        }

        public bool IsHorizontal()
        {
            return m_Orientation == Orientation.Horizontal;
        }

        public bool NotPlaced()
        {
            return (m_XPos < 0 || m_YPos < 0);
        }
    }
}
