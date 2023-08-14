using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Utility
{
    public class AutoIndexingDataTable : DataTable
    {
        public object this[int theColumn, int theRow] { get => getCell( theColumn, theRow ); set => setCell(theColumn, theRow, value); }

        public AutoIndexingDataTable(int theNumberOfRows, int theNumberOfColumns)
        {
            addColumns( theNumberOfColumns );
            addRows( theNumberOfRows );
        }

        private void addColumns(int theColumnCount, object theDefaultValue = null)
        {
            char theColumnChar = 'A';

            for (int anIdx = 0; anIdx <= theColumnCount; anIdx++)
            {
                DataColumn aCol = new DataColumn();
                if (anIdx == 0)
                {
                    aCol.AutoIncrement = true;
                    aCol.AutoIncrementSeed = 1;
                    aCol.ColumnName = "#";
                }
                else
                {
                    aCol.ColumnName = $"{theColumnChar++}";
                    aCol.DefaultValue = theDefaultValue;
                }
            }
        }

        private void addRows(int theRowCount)
        {
            for (int anIdx =0; anIdx < theRowCount; anIdx++)
            {
                var aRow = this.NewRow();
                this.Rows.Add( aRow );
            }
        }

        private object getCell(int theColumn, int theRow)
        {
            return this.Rows[theRow][theColumn];
        }

        private void setCell(int theColumn, int theRow, object theValue)
        {
            this.Rows[theRow][theColumn] = theValue;
        }
    }
}
