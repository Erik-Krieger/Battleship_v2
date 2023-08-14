using System.Windows.Controls;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel();
        }
    }
}
