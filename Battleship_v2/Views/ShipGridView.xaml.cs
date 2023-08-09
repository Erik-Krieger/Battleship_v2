using System.Windows.Controls;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views
{
    /// <summary>
    /// Interaction logic for ShipGridView.xaml
    /// </summary>
    public partial class ShipGridView : UserControl
    {
        public ShipGridView()
        {
            InitializeComponent();

            DataContext = new ShipGridViewModel();
        }
    }
}
