using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies
{
    public sealed class EnemyMedium : Enemy
    {
        /// <summary>
        /// 
        /// </summary>
        public EnemyMedium() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValidMoves"></param>
        /// <returns></returns>
        public override Position NextMove()
        {
            Position aNextMove;

            if ( m_LastMove != null && m_LastMove.WasHit )
            {
                aNextMove = findValidNeighbour(m_ValidMoves, m_LastMove);
                if (aNextMove != null ) return aNextMove;
            }

            var anIdx = aRng.Next(m_ValidMoves.Count);
            aNextMove = m_ValidMoves[anIdx];
            m_LastMove = aNextMove;
            return aNextMove;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValidMoves"></param>
        /// <param name="theMove"></param>
        /// <returns></returns>
        private static Position findValidNeighbour( List<Position> theValidMoves, Position theMove )
        {
            foreach ( Position aMove in theValidMoves )
            {
                if ( ( aMove.X == theMove.X - 1 || aMove.X == theMove.X + 1 ) && aMove.Y == theMove.Y ) return aMove;
                if ( ( aMove.Y == theMove.Y - 1 || aMove.Y == theMove.Y + 1 ) && aMove.X == theMove.X ) return aMove;
            }

            return null;
        }
    }
}
