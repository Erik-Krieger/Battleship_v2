using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using System.Collections.Generic;

namespace Battleship_v2.ViewModels
{
    public sealed class GameViewModel : BaseViewModel
    {
        private GameModel m_GameModel;

        public ShipGridViewModel OwnGrid { get; set; }
        private ShipGridViewModel m_OwnGrid;

        public ShipGridViewModel EnemyGrid { get; set; }
        private ShipGridViewModel m_EnemyGrid;

        public TargetInputViewModel TargetInput { get; set; } = new TargetInputViewModel();
        private TargetInputViewModel m_TargetInput;

        public GameViewModel(List<ushort> theOwnShipList, List<ushort> theEnemyShipList)
        {
            OwnGrid = new ShipGridViewModel(PlayerType.You, theOwnShipList);
            EnemyGrid = new ShipGridViewModel(PlayerType.Enemy, theEnemyShipList);

            m_GameModel = new GameModel(this);
        }

        public GameViewModel() : this(null, null) { }
    }
}
