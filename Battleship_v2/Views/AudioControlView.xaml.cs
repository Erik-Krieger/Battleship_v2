using Battleship_v2.ViewModels;
using System.Windows.Controls;

namespace Battleship_v2.Views
{
    /// <summary>
    /// Interaction logic for AudioControlView.xaml
    /// </summary>
    public partial class AudioControlView : UserControl
    {
        public AudioControlView()
        {
            InitializeComponent();
            DataContext = new AudioControlViewModel();
        }
    }
}
