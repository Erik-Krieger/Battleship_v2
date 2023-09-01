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

        public ICommand CmdSingleplayer
        {
            get => m_CmdSingleplayer ?? (m_CmdSingleplayer = new CommandHandler(() => WindowManagerService.ChangeView(new SingleplayerSetupViewModel())));
        }
        private ICommand m_CmdSingleplayer;
    }
}
