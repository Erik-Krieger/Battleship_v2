using System.Collections.ObjectModel;
using System.ComponentModel;
using Battleship_v2.Items;
using Battleship_v2.Models;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ShipGridRow> m_Grid;
        private ShipGridModel m_Model;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ShipGridRow> Grid
        {
            get => m_Grid;
            private set => m_Grid = value;
        }

        public ShipGridViewModel()
        {
            Grid = new ObservableCollection<ShipGridRow>();

            for (int anIdx = 0; anIdx < 10; anIdx++)
            {
                Grid.Add(new ShipGridRow(anIdx + 1));
            }
        }

        private bool isInBounds( int theXPos, int theYPos )
        {
            return ( theXPos >= 0 && theXPos <= 9 && theYPos >= 0 && theYPos <= 9 );
        }

        public void SetCell( int theXPos, int theYPos, char theValue )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return;
            }

            m_Grid[theYPos].SetColumn( theXPos, theValue );
        }

        public char GetCell( int theXPos, int theYPos )
        {
            if ( isInBounds( theXPos, theYPos ) )
            {
                return '\0';
            }

            return m_Grid[theYPos].GetColumn( theXPos );
        }
    }
}
