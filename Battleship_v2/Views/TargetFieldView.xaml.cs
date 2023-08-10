using System.Windows.Controls;
using Battleship_v2.ViewModels;

namespace Battleship_v2.Views
{
    /// <summary>
    /// Interaction logic for TargetFieldView.xaml
    /// </summary>
    public partial class TargetFieldView : UserControl
    {
        public TargetFieldView()
        {
            InitializeComponent();
            DataContext = new TargetInputViewModel();
        }
    }
}
