using System.Data;

namespace Battleship_v2.Items
{
    public class ShipGridRow
    {
        private readonly ShipGridCell[] m_ShipGridRow = new ShipGridCell[10];
        private int m_RowNumber;

        public string RowNumber
        {
            get => m_RowNumber.ToString();
            private set
            {
                if ( int.TryParse( value, out int aRownumber ) )
                {
                    m_RowNumber = aRownumber;
                }
            }
        }
        public string Column_0 { get => m_ShipGridRow[0].Letter; set => m_ShipGridRow[0].Letter = value; }
        public string Column_1 { get => m_ShipGridRow[1].Letter; set => m_ShipGridRow[1].Letter = value; }
        public string Column_2 { get => m_ShipGridRow[2].Letter; set => m_ShipGridRow[2].Letter = value; }
        public string Column_3 { get => m_ShipGridRow[3].Letter; set => m_ShipGridRow[3].Letter = value; }
        public string Column_4 { get => m_ShipGridRow[4].Letter; set => m_ShipGridRow[4].Letter = value; }
        public string Column_5 { get => m_ShipGridRow[5].Letter; set => m_ShipGridRow[5].Letter = value; }
        public string Column_6 { get => m_ShipGridRow[6].Letter; set => m_ShipGridRow[6].Letter = value; }
        public string Column_7 { get => m_ShipGridRow[7].Letter; set => m_ShipGridRow[7].Letter = value; }
        public string Column_8 { get => m_ShipGridRow[8].Letter; set => m_ShipGridRow[8].Letter = value; }
        public string Column_9 { get => m_ShipGridRow[9].Letter; set => m_ShipGridRow[9].Letter = value; }

        public ShipGridRow( int theRowNumber = -1 )
        {
            m_RowNumber = theRowNumber;

            m_ShipGridRow = new ShipGridCell[10];
            for ( int anIdx = 0; anIdx < 10; anIdx++ )
            {
                m_ShipGridRow[anIdx] = new ShipGridCell();
            }
        }

        private bool isInBounds( int theColumnNumber )
        {
            return ( theColumnNumber >= 0 && theColumnNumber <= 9 );
        }

        public void SetColumn( int theColumnNumber, char theValue )
        {
            if ( isInBounds( theColumnNumber ) )
            {
                return;
            }

            m_ShipGridRow[theColumnNumber].Letter = theValue.ToString();
        }

        public char GetColumn( int theColumnNumber )
        {
            if ( isInBounds( theColumnNumber ) )
            {
                return '\0';
            }

            return m_ShipGridRow[theColumnNumber].Letter[0];
        }
    }
}
