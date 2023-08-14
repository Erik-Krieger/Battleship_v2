using System.Diagnostics;
using System.Windows.Input;
using Battleship_v2.Models;
using Battleship_v2.Services;

namespace Battleship_v2.ViewModels
{
    public class TargetInputViewModel : Data
    {
        private string m_TargetString = string.Empty;
        private GameManagerService m_GameManager;

        public string TargetString
        {
            get => m_TargetString;
            set
            {
                SetField( ref m_TargetString, value );
            }
        }

        public TargetInputViewModel()
        {
            m_GameManager = GameManagerService.Instance;
            m_GameManager.InjectTargetInputViewModel( this );
        }

        public void ShootButtonPressed()
        {
            Debug.WriteLine( $"Value of TargetString: {TargetString}" );
            m_GameManager.ProcessShot( TargetString );
            TargetString = string.Empty;
        }

        private ICommand m_CmdShoot;
        public ICommand CmdShoot
        {
            get
            {
                return m_CmdShoot ?? ( m_CmdShoot = new CommandHandler( () => ShootButtonPressed(), () => CanExecute ) );
            }
        }
        public bool CanExecute
        {
            get => true;
        }
    }
}
