using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public class AudioControlViewModel : BaseViewModel
    {
        public Visibility WindowVisibility
        {
            get => m_WindowVisibility;
            set
            {
                m_WindowVisibility = value;
                NotifyPropertyChanged(nameof(WindowVisibility));
            }
        }
        private Visibility m_WindowVisibility = Visibility.Visible;

        public AudioControlViewModel() { }

        public ICommand CmdPButtonPressed
        {
            get => m_CmdPButtonPressed ?? new CommandHandler(() => Debug.WriteLine("Pressed"));
            /*get => m_CmdPButtonPressed ?? new CommandHandler(
                () =>
                {
                    WindowVisibility = WindowVisibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                    Debug.WriteLine("Changed Visibility");
                }
            );*/
        }
        private ICommand m_CmdPButtonPressed;

        public ICommand CmdOpenMultiplayerMenu
        {
            get => m_CmdOpenMultiplayerMenu ?? new CommandHandler(() => WindowManagerService.OpenMultiplayerMenu());
        }
        private ICommand m_CmdOpenMultiplayerMenu;
    }
}
