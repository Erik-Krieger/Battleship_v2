using System.ComponentModel;
using System.Data;
using Battleship_v2.Models;
using Battleship_v2.Services;

namespace Battleship_v2.ViewModels
{
    public class ShipGridViewModel : PropertyChangeHandler
    {
        private ShipGridModel m_Model;
        private GameManagerService m_GameManager;

        public DataTable Grid
        {
            get => m_Grid;
            set => SetProperty( ref m_Grid, value );
        }
        private DataTable m_Grid;

        public ShipGridViewModel()
        {
            m_Model = new ShipGridModel(this);
        }
    }
}
