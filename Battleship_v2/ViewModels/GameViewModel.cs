using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class GameViewModel : PropertyChangeHandler
    {
        private NavigationViewModel m_NavigationViewModel;
        private GameModel m_GameModel;

        public ShipGridViewModel OwnGrid { get; set; } = new ShipGridViewModel( PlayerType.You );
        private ShipGridViewModel m_OwnGrid;

        public ShipGridViewModel EnemyGrid { get; set; } = new ShipGridViewModel( PlayerType.Enemy );
        private ShipGridViewModel m_EnemyGrid;

        public TargetInputViewModel TargetInput { get; set; } = new TargetInputViewModel();
        private TargetInputViewModel m_TargetInput;

        public GameViewModel( NavigationViewModel theNavigationViewModel )
        {
            m_NavigationViewModel = theNavigationViewModel;
            m_GameModel = new GameModel( this );
        }
    }
}
