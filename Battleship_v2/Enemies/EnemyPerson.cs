using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Battleship_v2.Services;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies
{
    public class EnemyPerson : Enemy
    {
        public override Position NextMove()
        {
            var np = NetworkService.Instance.NetworkPeer;

            lock (np.MessageQueue)
            {
                if (np.GetMessage(out string aMessage))
                {
                    return new Position(aMessage);
                }


                return null;
            }
        }
    }

    public void MakeMoveOnMessage()
    {
        var np = NetworkService.Instance.NetworkPeer;

        lock (np.MessageQueue)
        {

        }
    }
}
