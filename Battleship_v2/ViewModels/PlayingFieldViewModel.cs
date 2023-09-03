using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Battleship_v2.ViewModels
{
    public sealed class PlayingFieldViewModel : BaseViewModel
    {
        public PlayingFieldModel Model { get; set; }
        public PlayerType Owner { get; }
        private GameManagerService m_GameManager;

        public DataTable Grid
        {
            get => m_Grid;
            set => SetProperty(ref m_Grid, value);
        }
        private DataTable m_Grid;

        public DataGridCellInfo CurrentCell
        {
            get => m_CurrentCell;
            set
            {
                // If this is not the opponent grid terminate.
                if (Owner == PlayerType.You) return;

                // Set the value of the current cell to the backing field.
                m_CurrentCell = value;

                // Check if the Column or the Item is null, in that case termiante.
                if (m_CurrentCell.Column is null || m_CurrentCell.Item is null) return;
                
                // Call the processing Method in the Model.
                Model.GridCellClicked(m_CurrentCell.Column.DisplayIndex, ((DataRowView)m_CurrentCell.Item)[0]);
            }
        }
        private DataGridCellInfo m_CurrentCell;

        public List<Ship> Ships { get; set; }

        public PlayingFieldViewModel(PlayerType theOwner, List<ushort> theShipList)
        {
            Owner = theOwner;
            Model = new PlayingFieldModel(this, theOwner == PlayerType.You, theShipList);
            if (theOwner == PlayerType.You)
            {
                //Model.DrawAllShips();
            }
        }
    }
}
