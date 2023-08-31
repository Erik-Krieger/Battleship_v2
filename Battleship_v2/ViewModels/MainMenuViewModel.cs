using Battleship_v2.Services;
using Battleship_v2.Utility;
using System;
using System.Windows;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class MainMenuViewModel : BaseViewModel
    {
        public MainMenuViewModel() { }

        public Action Close { get; set; }

        public ICommand CmdSingleplayer
        {
            get => m_CmdSingleplayer ?? (m_CmdSingleplayer = new CommandHandler(() => WindowManagerService.ChangeView(new SingleplayerSetupViewModel())));
        }
        private ICommand m_CmdSingleplayer;

        public ICommand CmdMultiplayer
        {
            get => m_CmdMultiplayer ?? (m_CmdMultiplayer = new CommandHandler(() => WindowManagerService.ChangeView(new MultiplayerSetupViewModel())));
        }
        private ICommand m_CmdMultiplayer;

        public ICommand CmdClose
        {
            get => m_CmdClose ?? (m_CmdClose = new CommandHandler(() => Application.Current.Shutdown()));
        }
        private ICommand m_CmdClose;
    }
}
