using Battleship_v2.Services;
using Battleship_v2.Utility;
using System.Windows;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public class BaseViewModel : PropertyChangeHandler
    {
        /// <summary>
        /// A bindable Command to return to the main menu
        /// </summary>
        public ICommand CmdOpenMainMenu
        {
            get => m_CmdOpenMainMenu ?? new CommandHandler(() => WindowManagerService.OpenMainMenu());
        }
        private ICommand m_CmdOpenMainMenu;

        /// <summary>
        /// A bindable Command to terminate the application.
        /// </summary>
        public ICommand CmdClose
        {
            get => m_CmdClose ?? (m_CmdClose = new CommandHandler(() => Application.Current.Shutdown()));
        }
        private ICommand m_CmdClose;

        /// <summary>
        /// A bindable Command to open the multiplayer menu.
        /// </summary>
        public ICommand CmdOpenMultiplayerMenu
        {
            get => m_CmdOpenMultiplayerMenu ?? new CommandHandler(() => WindowManagerService.OpenMultiplayerMenu());
        }
        private ICommand m_CmdOpenMultiplayerMenu;

        /// <summary>
        /// The default constructor for the BaseViewModel
        /// </summary>
        public BaseViewModel() { }
    }
}
