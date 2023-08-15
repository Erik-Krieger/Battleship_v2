using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship_v2.Utility;

namespace Battleship_v2.Enemy
{
    public abstract class Enemy
    {
        protected Random aRng = new Random();

        public Enemy() { }

        public abstract Move NextMove(List<Move> theValidMoves);
    }
}
