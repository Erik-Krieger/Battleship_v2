using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship_v2.Services;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class GameModel
    {
        public GameViewModel ViewModel { get; set; }
        private GameViewModel m_ViewModel;

        public GameModel(GameViewModel theGameViewModel)
        {
            ViewModel = theGameViewModel;
            GameManagerService.Instance.InjectShipGridModel(ViewModel.OwnGrid.Model, true);
            GameManagerService.Instance.InjectShipGridModel(ViewModel.EnemyGrid.Model, false);
        }


    }
}
