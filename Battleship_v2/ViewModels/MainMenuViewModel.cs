using System.Windows.Input;
using Battleship_v2.Utility;

namespace Battleship_v2.ViewModels
{
    public class MainMenuViewModel
    {
        public MainMenuViewModel() { }

        public ICommand CmdSingleplayer
        {
            get => m_CmdSingleplayer ?? ( m_CmdSingleplayer = new CommandHandler( () => { } ) );
        }
        private ICommand m_CmdSingleplayer;

        public ICommand CmdMultiplayer
        {
            get => m_CmdMultiplayer ?? ( m_CmdMultiplayer = new CommandHandler( () => { } ) );
        }
        private ICommand m_CmdMultiplayer;

        public ICommand CmdClose
        {
            get => m_CmdClose ?? ( m_CmdClose = new CommandHandler( () => { } ) );
        }
        private ICommand m_CmdClose;
    }
}
