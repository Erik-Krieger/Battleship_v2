using System.ComponentModel;
using System.Data;
using Battleship_v2.Models;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : Data
    {
        private ShipGridModel m_Model;

        private DataTable m_Grid;
        public DataTable Grid
        {
            get
            {
                return m_Grid;
            }
            set
            {
                m_Grid = value;
                NotifyPropertyChanged( nameof( Grid ) );
            }
        }

        public ShipGridViewModel()
        {
            m_Model = new ShipGridModel(this);
            Grid = new DataTable();

            char theColumnLetter = 'A';

            // Generate Columns
            for ( int anIdx = 0; anIdx < 11; anIdx++ )
            {
                var aCol = new DataColumn();

                if ( anIdx == 0 )
                {
                    aCol.ColumnName = "#";
                    aCol.AutoIncrement = true;
                    aCol.AutoIncrementSeed = 1;
                }
                else
                {
                    aCol.ColumnName = $"{theColumnLetter++}";
                    aCol.DefaultValue = "w";
                }
                Grid.Columns.Add( aCol );
            }

            // Generate Rows
            for (int anIdx = 0; anIdx < 10; anIdx++ )
            {
                DataRow aRow = Grid.NewRow();
                Grid.Rows.Add( aRow );
            }

            // Fill the Grid
            for ( int anYPos = 0; anYPos < 10; anYPos++ )
            {
                var aRow = Grid.Rows[anYPos];
                for ( int anXPos = 1; anXPos <= 10; anXPos++ )
                {
                    var aCol = Grid.Columns[anXPos];
                    aRow[aCol] = "w";
                }
            }

            m_Model.DrawAllShips();
        }

        private bool isInBounds( int theXPos, int theYPos )
        {
            return ( theXPos >= 0 && theXPos <= 9 && theYPos >= 0 && theYPos <= 9 );
        }

        public void SetCell( int theXPos, int theYPos, char theValue )
        {
            // We need to increment here, since Column zero contains the row number
            // and when we say theXPos = 0, we refer to the first column of the playing field.
            theXPos++;

            if ( !isInBounds( theXPos, theYPos ) )
            {
                return;
            }

            var aRow= m_Grid.Rows[theYPos];
            aRow[m_Grid.Columns[theXPos]] = $"{theValue}";
        }

        public char GetCell( int theXPos, int theYPos )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return '\0';
            }

            var aRow = m_Grid.Rows[theYPos];
            var aCell = aRow[m_Grid.Columns[theXPos]];
            var aCellData = (string)aCell;
            return aCellData[0];
        }
    }
}
