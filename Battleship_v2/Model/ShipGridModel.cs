using System.Collections.Generic;
using System.Windows.Documents;
using Battleship_v2.Ships;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class ShipGridModel
    {
        private ShipGridViewModel m_ViewModel;
        private TargetInputViewModel m_TargetInputViewModel;
        private List<Ship> m_Ships;

        public ShipGridModel(ShipGridViewModel theViewModel)
        {
            m_ViewModel = theViewModel;

            m_Ships = new List<Ship>()
            {
                new Carrier(),

            };
        }
    }
}
