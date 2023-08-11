using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Battleship_v2.Services
{
    public class ShotManagerService : CommandHandler
    {
        private ICommand m_ShootCommand;

        public ICommand ShootCommand => m_ShootCommand?.Invoke( Shoot(), null );

        public void Shoot()
        {

        }

        public bool CanExecute( object parameter )
        {
            throw new NotImplementedException();
        }

        public void Execute( object parameter )
        {
            throw new NotImplementedException();
        }

        public ShotManagerService() { }

    }
}
