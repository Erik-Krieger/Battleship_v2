using Battleship_v2.Models;
using Battleship_v2.Services;
using Battleship_v2.Utility;
using System.Windows.Input;

namespace Battleship_v2.ViewModels
{
    public sealed class TargetInputViewModel : BaseViewModel
    {
        /// <summary>
        /// A bindable String that contains the cell to shoot at.
        /// </summary>
        public string TargetString
        {
            get => m_TargetString;
            set => SetProperty(ref m_TargetString, value);
        }
        private string m_TargetString = string.Empty;

        /// <summary>
        /// The correspomnding Model to this ViewModel
        /// </summary>
        public TargetInputModel Model { get; set; }

        /// <summary>
        /// The default constructor for the TargetInputViewModel
        /// </summary>
        public TargetInputViewModel()
        {
            Model = new TargetInputModel(this);
        }

        /// <summary>
        /// A bindable Command that initiates a shot onto the Grid.
        /// </summary>
        public ICommand CmdShoot
        {
            get => m_CmdShoot ?? (m_CmdShoot = new CommandHandler(
                () => Model.ShootButtonPressed(),
                () => GameManagerService.Instance.YourTurn));
        }
        private ICommand m_CmdShoot;
    }
}
