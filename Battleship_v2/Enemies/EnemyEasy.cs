using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies
{
    public class EnemyEasy : Enemy
    {
        public EnemyEasy() {}

        public override Position NextMove()
        {
            return getRandomMove();
        }
    }
}
