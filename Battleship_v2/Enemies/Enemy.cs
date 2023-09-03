using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemies
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Person,
    }

    public abstract class Enemy
    {
        protected Random aRng = new Random();

        protected List<Position> m_ValidMoves;

        public Enemy()
        {
            m_ValidMoves = new List<Position>( 100 );
            for ( int aRow = 0; aRow < 10; aRow++ )
            {
                for ( int aCol = 0; aCol < 10; aCol++ )
                {
                    m_ValidMoves.Add( new Position( aCol, aRow ) );
                }
            }
        }

        public abstract Position NextMove();

        protected private Position getRandomMove()
        {
            int anIndex = aRng.Next(m_ValidMoves.Count);
            Position aMove = m_ValidMoves[anIndex];
            m_ValidMoves.RemoveAt(anIndex);
            
            return aMove;
        }
    }
}
