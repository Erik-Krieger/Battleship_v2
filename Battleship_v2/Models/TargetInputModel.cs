using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship_v2.Services;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class TargetInputModel
    {
        public TargetInputViewModel ViewModel { get; set; }
        public GameManagerService GameManager { get; set; }
        public TargetInputModel(TargetInputViewModel theViewModel)
        {
            ViewModel = theViewModel;
            GameManager = GameManagerService.Instance.InjectTargetInputModel( this );
        }

        public void ShootButtonPressed()
        {
            if ( !GameManagerService.Instance.YourTurn ) return;
            GameManagerService.Instance.ProcessShot( ViewModel.TargetString );
            ViewModel.TargetString = string.Empty;
        }
    }
}
