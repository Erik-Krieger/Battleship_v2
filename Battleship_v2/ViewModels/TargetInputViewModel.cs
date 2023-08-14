using System.Diagnostics;
using System.Windows.Input;
using Battleship_v2.Models;
using Battleship_v2.Services;

namespace Battleship_v2.ViewModels
{
    public class TargetInputViewModel : PropertyChangeHandler
    {
        private string m_TargetString = string.Empty;
        public TargetInputModel Model { get; set; }

        public string TargetString
        {
            get => m_TargetString;
            set
            {
                //SetProperty( ref m_TargetString, value );
                m_TargetString = value;
                NotifyPropertyChanged(nameof(TargetString));
            }
        }

        public TargetInputViewModel()
        {
            Model = new TargetInputModel(this);
        }

        public ICommand CmdShoot { get => m_CmdShoot ?? (m_CmdShoot = new CommandHandler( () => Model.ShootButtonPressed(), () => CanExecute ) ); }
        private ICommand m_CmdShoot;

        public bool CanExecute { get => true; }
    }
}
