using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using Battleship_v2.Items;
using Battleship_v2.Models;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : INotifyPropertyChanged
    {
        private ShipGridModel m_Model;

        public DataTable Grid;

        public event PropertyChangedEventHandler PropertyChanged;

        public ShipGridViewModel()
        {
            Grid = new DataTable();
            char theColumnLetter = 'A';

            for (int anIdx = 0; anIdx < 10; anIdx++)
            {
                Grid.Columns.Add( new DataColumn( $"{theColumnLetter++}" ) );
                Grid.Rows.Add();
            }

            for (int anIdx = 0; anIdx < 10; anIdx++)
            {
                for (int anJdx = 0; anJdx < 10; anJdx++)
                {
                    Grid[0][0];
                }
            }
        }

        private bool isInBounds( int theXPos, int theYPos )
        {
            return ( theXPos >= 0 && theXPos <= 9 && theYPos >= 0 && theYPos <= 9 );
        }

        public void SetCell( int theXPos, int theYPos, char theValue )
        {
            if ( !isInBounds( theXPos, theYPos ) )
            {
                return;
            }

            //m_Grid[theYPos].SetColumn( theXPos, theValue );
        }

        public char GetCell( int theXPos, int theYPos )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return '\0';
            }

            //return m_Grid[theYPos].GetColumn( theXPos );
            return '\0';
        }
    }
}
