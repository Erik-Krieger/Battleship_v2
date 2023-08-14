using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.ViewModels
{
    public class GameViewModel : PropertyChangeHandler
    {
        public ShipGridViewModel OwnGrid { get; set; }
        private ShipGridViewModel m_OwnGrid;

        public ShipGridViewModel EnemyGrid { get; set; }
        private ShipGridViewModel m_EnemyGrid;

        public TargetInputViewModel TargetInput { get; set; }
        private TargetInputViewModel m_TargetInput;

        public GameViewModel()
        {

        }
    }
}
