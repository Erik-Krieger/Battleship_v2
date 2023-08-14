using Battleship_v2.ViewModels;

namespace Battleship_v2.Models
{
    public class MainMenuModel
    {
        public MainMenuViewModel ViewModel { get; set; }

        public MainMenuModel(MainMenuViewModel theViewModel)
        {
            ViewModel = theViewModel;
        }

        public void PlaySinglePlayer()
        {

        }
    }
}
