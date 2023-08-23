using System;
using System.Windows;
using System.Windows.Input;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class MainMenuViewModel
    {
        public MainMenuViewModel() {}

        public Action Close { get; set; }

        public ICommand CmdSingleplayer
        {
            get => m_CmdSingleplayer ?? ( m_CmdSingleplayer = new CommandHandler( () => { openSingleplayer(); } ) );
        }
        private ICommand m_CmdSingleplayer;

        public ICommand CmdMultiplayer
        {
            get => m_CmdMultiplayer ?? ( m_CmdMultiplayer = new CommandHandler( () => { openMultiplayer(); } ) );
        }
        private ICommand m_CmdMultiplayer;

        public ICommand CmdClose
        {
            get => m_CmdClose ?? ( m_CmdClose = new CommandHandler( () => { Application.Current.Shutdown(); } ) );
        }
        private ICommand m_CmdClose;

        private void openSingleplayer( object theObject = null )
        {
            WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new SingleplayerSetupViewModel();
        }

        private void openMultiplayer( object theObject = null )
        {
            WindowManagerService.Instance.NavigationViewModel.SelectedViewModel = new MultiplayerSetupViewModel();
        }
    }
}
