using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Battleship_v2.Models;

namespace Battleship_v2.ViewModels
{
    public class TargetInputViewModel : Data
    {
        private string m_TargetString = string.Empty;
        private ShipGridModel m_ShipGridModel;

        public string TargetString
        {
            get => m_TargetString;
            set
            {
                SetField(ref m_TargetString, value);
            }
        }

        public TargetInputViewModel() {}

        public void CmdShootButtonPressed()
        {
            TargetString = string.Empty;
            Debug.WriteLine( $"Value of TargetString: {TargetString}" );
        }

        private ICommand m_CmdShoot;
        public ICommand CmdShoot
        {
            get
            {
                return m_CmdShoot ?? ( m_CmdShoot = new CommandHandler( () => CmdShootButtonPressed(), () => CanExecute ) );
            }
        }
        public bool CanExecute
        {
            get => true;
        }

        public void InjectModel( ShipGridModel theShipGridModel )
        {
            m_ShipGridModel = theShipGridModel;
        }
    }
}
