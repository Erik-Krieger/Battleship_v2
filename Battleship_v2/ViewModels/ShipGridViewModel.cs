using System.Collections.ObjectModel;
using System.ComponentModel;
using Battleship_v2.Items;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ItemShipViewModel> m_Grid;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ItemShipViewModel> Grid
        {
            get => m_Grid;
            private set => m_Grid = value;
        }

        public ShipGridViewModel()
        {
            Grid = new ObservableCollection<ItemShipViewModel>()
            {
                new ItemShipViewModel(1),
                new ItemShipViewModel(2),
                new ItemShipViewModel(3),
                new ItemShipViewModel(4),
                new ItemShipViewModel(5),
                new ItemShipViewModel(6),
                new ItemShipViewModel(7),
                new ItemShipViewModel(8),
                new ItemShipViewModel(9),
                new ItemShipViewModel(10)
            };
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
