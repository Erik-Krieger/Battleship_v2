using System.Data;
using System.Windows.Controls;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class ShipGridModel
    {
        public const int GRID_SIZE = 10;

        public ShipGridViewModel ViewModel { get; private set; }

        public ShipGridModel(ShipGridViewModel theViewModel)
        {
            ViewModel = theViewModel;
            GameManagerService.Instance.InjectShipGridModel(this);

            ViewModel.Grid = new DataTable();

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
                ViewModel.Grid.Columns.Add( aCol );
            }

            // Generate Rows
            for ( int anIdx = 0; anIdx < 10; anIdx++ )
            {
                DataRow aRow = ViewModel.Grid.NewRow();
                ViewModel.Grid.Rows.Add( aRow );
            }
        }

        private bool isInBounds( int theXPos, int theYPos )
        {
            // In case a change breaks hitting any shot in the last column, it's probably due this line.
            // The X-Position bounds check might need to be adjusted. At the moment we're pretending that the first column i.e.
            // the one containing the row number, does not exist. This might not always be possible in the future.
            // Should that be the case, change the bounds check for "theXPos" to the range [1,10] or [1,11).
            return ( theXPos >= 0 && theXPos < GRID_SIZE && theYPos >= 0 && theYPos < GRID_SIZE );
        }

        public void SetCell( int theXPos, int theYPos, char theValue )
        {
            if ( !isInBounds( theXPos, theYPos ) )
            {
                return;
            }

            // We need to increment here, since Column zero contains the row number
            // and when we say theXPos = 0, we refer to the first column of the playing field.
            theXPos++;

            var aRow = ViewModel.Grid.Rows[theYPos];
            aRow[ViewModel.Grid.Columns[theXPos]] = $"{theValue}";
        }

        public char GetCell( int theXPos, int theYPos )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return '\0';
            }

            var aRow = ViewModel.Grid.Rows[theYPos];
            var aCell = aRow[ViewModel.Grid.Columns[theXPos]];
            return ((string)aCell)[0];
        }
    }
}