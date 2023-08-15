using System.Windows.Controls;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public NavigationView()
        {
            InitializeComponent();
            var aViewModel = new NavigationViewModel();
            aViewModel.SelectedViewModel = new MainMenuViewModel( aViewModel );
            DataContext = aViewModel;
        }
    }
}
