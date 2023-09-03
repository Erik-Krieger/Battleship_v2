using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Controls;

namespace Battleship_v2.ViewModels
{
    public sealed class ShipGridViewModel : BaseViewModel
    {
        public ShipGridModel Model { get; set; }
        public PlayerType Owner { get; }
        private GameManagerService m_GameManager;

        public DataTable Grid
        {
            get => m_Grid;
            set => SetProperty(ref m_Grid, value);
        }
        private DataTable m_Grid;

        public int SelectedRow
        {
            get => m_SelectedRow;
            set
            {
                m_SelectedRow = value;
                Debug.WriteLine($"Selected Index: {1}, {SelectedRow}");
                NotifyPropertyChanged(nameof(SelectedRow));
            }
        }
        private int m_SelectedRow;

        public DataGridColumn SelectedColumn { get; set; }

        public List<Ship> Ships { get; set; }

        public ShipGridViewModel(PlayerType theOwner, List<ushort> theShipList)
        {
            Owner = theOwner;
            Model = new ShipGridModel(this, theOwner == PlayerType.You, theShipList);
            if (theOwner == PlayerType.You)
            {
                //Model.DrawAllShips();
            }
        }
    }
}
