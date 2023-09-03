using Battleship_v2.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Battleship_v2.Enemies
{
    public sealed class EnemyMedium : Enemy
    {
        /// <summary>
        /// 
        /// </summary>
        public EnemyMedium() { }

        private bool isSeries = false;

        private List<Position> m_LastMoves = new List<Position>(100);
        private List<Position> m_NextMoves = new List<Position>(5);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Position NextMove()
        {
            Position aMove;

            if (m_LastMoves.Count == 0)
            {
                aMove = getRandomMove();
                m_LastMoves.Add(aMove);
                return aMove;
            }

            if (wasLastMoveHit())
            {
                // Find all valid neighbouring moves.
                if (setValidNeighboursAsNextMove(m_LastMoves.Last())) {
                    // Pick the first of them.
                    aMove = m_NextMoves.First();
                    // Remove the Move from the next moves list
                    m_NextMoves.Remove(aMove);
                    // Remove the Move from the valid moves list
                    m_ValidMoves.Remove(aMove);
                    // Add it to the last Moves list.
                    m_LastMoves.Add(aMove);
                    // Return the Move.
                    return aMove;
                }
            }
            else
            {
                m_LastMoves.Remove(m_LastMoves.Last());
            }

            aMove = getRandomMove();
            m_LastMoves.Add(aMove);
            return aMove;
        }

        private bool wasLastMoveHit()
        {
            if (m_LastMoves.Count == 0) return false;

            return m_LastMoves.Last().WasHit;
        }

        private bool wasHitInRecentMoves()
        {
            for (int i = m_LastMoves.Count - 1; i >= 0; i--)
            {
                if (m_LastMoves[i].WasHit)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theValidMoves"></param>
        /// <param name="theMove"></param>
        /// <returns></returns>
        private bool setValidNeighboursAsNextMove(Position theMove)
        {
            List<Position> aList = theMove.GetNeighbours(0, 10);

            aList = aList.Intersect(m_ValidMoves) as List<Position>;

            m_NextMoves.InsertRange(0, aList);

            return aList.Count > 0;
        }
    }
}
