using System.Windows;
using Battleship_v2.ViewModels;

namespace Battleship_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var aViewModel = new NavigationViewModel();
            aViewModel.SelectedViewModel = new MainMenuViewModel( aViewModel );
            DataContext = aViewModel;
        }
    }
}
