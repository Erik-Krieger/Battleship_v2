using System.Collections.Generic;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemy
{
    public sealed class EnemyMedium : Enemy
    {
        /// <summary>
        /// 
        /// </summary>
        private Move m_LastMove;

        /// <summary>
        /// 
        /// </summary>
        public EnemyMedium() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValidMoves"></param>
        /// <returns></returns>
        public override Move NextMove( List<Move> theValidMoves )
        {
            Move aNextMove;

            if ( m_LastMove != null && m_LastMove.WasHit )
            {
                aNextMove = findValidNeighbour(theValidMoves, m_LastMove);
                if (aNextMove != null ) return aNextMove;
            }

            var anIdx = aRng.Next(theValidMoves.Count);
            aNextMove = theValidMoves[anIdx];
            m_LastMove = aNextMove;
            return aNextMove;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValidMoves"></param>
        /// <param name="theMove"></param>
        /// <returns></returns>
        private static Move findValidNeighbour( List<Move> theValidMoves, Move theMove )
        {
            foreach ( Move aMove in theValidMoves )
            {
                if ( ( aMove.X == theMove.X - 1 || aMove.X == theMove.X + 1 ) && aMove.Y == theMove.Y ) return aMove;
                if ( ( aMove.Y == theMove.Y - 1 || aMove.Y == theMove.Y + 1 ) && aMove.X == theMove.X ) return aMove;
            }

            return null;
        }
    }
}
