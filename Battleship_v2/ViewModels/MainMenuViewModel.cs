using System;
using System.Windows.Input;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class MainMenuViewModel
    {
        private readonly NavigationViewModel m_NavigationViewModel;

        public MainMenuViewModel( NavigationViewModel theNavigationViewModel )
        {
            m_NavigationViewModel = theNavigationViewModel;
        }

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
            get => m_CmdClose ?? ( m_CmdClose = new CommandHandler( () => { } ) );
        }
        private ICommand m_CmdClose;

        private void openSingleplayer( object theObject = null )
        {
            m_NavigationViewModel.SelectedViewModel = new SingleplayerSetupViewModel(m_NavigationViewModel);
        }

        private void openMultiplayer( object theObject = null )
        {
            m_NavigationViewModel.SelectedViewModel = new MultiplayerSetupViewModel(m_NavigationViewModel);
        }
    }
}
