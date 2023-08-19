using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : PropertyChangeHandler
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
                Debug.WriteLine($"Selected Index: {SelectedRow}, {SelectedColumn}");
                NotifyPropertyChanged(nameof(SelectedRow));
            }
        }
        private int m_SelectedRow;

        public int SelectedColumn { get; set; }

        public DataGridItem SelectedItem
        {
            get => m_SelectedItem;
            set
            {
                m_SelectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
            }
        }
        private DataGridItem m_SelectedItem;

        public List<Ship> Ships { get; set; }

        public ShipGridViewModel(PlayerType theOwner)
        {
            Owner = theOwner;
            Model = new ShipGridModel(this, theOwner == PlayerType.You);
            if (theOwner == PlayerType.You)
            {
                //Model.DrawAllShips();
            }
        }
    }
}
