using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class PatrolBoat : Ship
    {
        const char LETTER = 'p';
        const int LENGTH = 2;

        public PatrolBoat() : base(LETTER, LENGTH) { }
    }
}
