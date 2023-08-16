using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies
{
    public class EnemyEasy : Enemy
    {
        public EnemyEasy() {}

        public override Position NextMove()
        {
            int anIndex = aRng.Next( m_ValidMoves.Count );
            Position aNextMove =  m_ValidMoves[anIndex];
            m_ValidMoves.Remove( aNextMove );
            return aNextMove;
        }
    }
}
