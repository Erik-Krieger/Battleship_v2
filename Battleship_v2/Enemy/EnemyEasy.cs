using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemy
{
    public class EnemyEasy : Enemy
    {
        private List<Move> m_ValidMoves;
        public EnemyEasy()
        {
            m_ValidMoves = new List<Move>( 100 );
            for ( int aRow = 0; aRow < 10; aRow++ )
            {
                for ( int aCol = 0; aCol < 10; aCol++ )
                {
                    m_ValidMoves.Add( new Move( aCol, aRow ) );
                }
            }
        }

        public override Move NextMove( List<Move> theValidMoves = null )
        {
            theValidMoves = m_ValidMoves;
            int anIndex = aRng.Next( theValidMoves.Count );
            Move aNextMove =  theValidMoves[anIndex];
            m_ValidMoves.Remove( aNextMove );
            return aNextMove;
        }
    }
}
