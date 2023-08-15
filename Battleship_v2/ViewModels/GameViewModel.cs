using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class GameViewModel : PropertyChangeHandler
    {
        private NavigationViewModel m_NavigationViewModel;

        public ShipGridViewModel OwnGrid { get; set; }
        private ShipGridViewModel m_OwnGrid;

        public ShipGridViewModel EnemyGrid { get; set; }
        private ShipGridViewModel m_EnemyGrid;

        public TargetInputViewModel TargetInput { get; set; }
        private TargetInputViewModel m_TargetInput;

        public GameViewModel(NavigationViewModel theNavigationViewModel)
        {
            m_NavigationViewModel = theNavigationViewModel;
        }
    }
}
