using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.ViewModels
{
    public class GameViewModel : PropertyChangeHandler
    {
        private ShipGridViewModel m_OwnGrid;
        private ShipGridViewModel m_EnemyGrid;
        private TargetInputViewModel m_TargetInput;

        public ShipGridViewModel OwnGrid { get; set; }
        public ShipGridViewModel EnemyGrid { get; set; }
        public TargetInputViewModel TargetInput { get; set; }

        public GameViewModel()
        {

        }
    }
}
