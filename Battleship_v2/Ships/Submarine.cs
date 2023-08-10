using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship_v2.Ships
{
    sealed public class Submarine : Ship
    {
        const char LETTER = 's';
        const int LENGTH = 3;

        public Submarine() : base(LETTER, LENGTH) { }
    }
}
