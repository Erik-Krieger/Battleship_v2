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

        public Ship( char theLetterRepresenation, int theLength )
        {
            m_Letter = theLetterRepresenation;
            m_Length = theLength;
        }

        private static bool isLegalPosition( int theXPos, int theYPos, int theLength, Orientation theOrientation)
        {
            if ( theOrientation == Orientation.Horizontal)
            {
                return (theXPos >= 0 && theYPos + theLength < 10 && theYPos >= 0 && theYPos < 10);
            }
            else
            {
                return (theXPos >= 0 && theXPos < 10 && theYPos >= 0 && theYPos + theLength < 10);
            }
        }

        public bool IsHit(int theXPos, int theYPos)
        {
            if ( isLegalPosition(theXPos, theYPos, m_Length, m_Orientation) )
            {
                return false;
            }

            return false;
        }
    }
}
