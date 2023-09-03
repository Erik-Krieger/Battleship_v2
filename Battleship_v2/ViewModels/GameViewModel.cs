using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Ships;
using System.Collections.Generic;

namespace Battleship_v2.ViewModels
{
    public sealed class GameViewModel : BaseViewModel
    {
        private GameModel m_GameModel;

        public PlayingFieldViewModel OwnGrid { get; set; }
        private PlayingFieldViewModel m_OwnGrid;

        public PlayingFieldViewModel EnemyGrid { get; set; }
        private PlayingFieldViewModel m_EnemyGrid;

        public TargetInputViewModel TargetInput { get; set; } = new TargetInputViewModel();
        private TargetInputViewModel m_TargetInput;

        public GameViewModel(List<ushort> theOwnShipList, List<ushort> theEnemyShipList)
        {
            OwnGrid = new PlayingFieldViewModel(PlayerType.You, theOwnShipList);
            EnemyGrid = new PlayingFieldViewModel(PlayerType.Enemy, theEnemyShipList);

            m_GameModel = new GameModel(this);
        }

        public GameViewModel() : this(null, null) { }
    }
}
