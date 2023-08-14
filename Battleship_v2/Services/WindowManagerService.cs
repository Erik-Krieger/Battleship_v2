namespace Battleship_v2.Services
{
    public enum Window
    {
        MainMenu,
        MultiplayerHost,
        MultiplayerClient,
        MainGame,
        GameOver,
    }

    public class WindowManagerService
    {
        public static WindowManagerService Instance { get; private set; } = new WindowManagerService();
        public Window CurrentWindow { get; private set; } = Window.MainMenu;

        private WindowManagerService() { }

        public void SwapScreens(Window theWindow)
        {

        }
    }
}
